#include "agent.hpp"

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>
#include <hv/hloop.h>
#include <spdlog/spdlog.h>

#include <exception>
#include <memory>
#include <string>
#include <string_view>

#include "available_buffs.hpp"
#include "message.hpp"
#include "player.hpp"

constexpr unsigned int kMinDelayMs{10};

namespace thuai8_agent {

Agent::Agent(std::string_view token, const hv::EventLoopPtr& event_loop,
             int loop_interval_ms)
    : token_(token),
      event_loop_(event_loop),
      ws_client_(std::make_unique<hv::WebSocketClient>(event_loop)),
      timer_id_(event_loop->setInterval(loop_interval_ms,
                                        [this](hv::TimerID) { Loop(); })) {
  reconn_setting_t reconn_setting{};
  reconn_setting.delay_policy = 0;
  reconn_setting.min_delay = kMinDelayMs;
  ws_client_->setReconnect(&reconn_setting);

  ws_client_->onmessage = [this](std::string_view msg) { OnMessage(msg); };
}

Agent::~Agent() { event_loop_->killTimer(timer_id_); }

void Agent::Connect(const std::string& server) {
  ws_client_->open(server.data());
}

void Agent::MoveForward(float distance) const {
  spdlog::info("{}.MoveForward({})", *this, distance);
  ws_client_->send(Message::PerformMove(token_, "FORTH", distance));
}

void Agent::MoveBackward(float distance) const {
  spdlog::info("{}.MoveBackward({})", *this, distance);
  ws_client_->send(Message::PerformMove(token_, "BACK", distance));
}

void Agent::TurnClockwise(int angle) const {
  spdlog::info("{}.TurnClockwise({})", *this, angle);
  ws_client_->send(Message::PerformTurn(token_, "CLOCKWISE", angle));
}

void Agent::TurnCounterClockwise(int angle) const {
  spdlog::info("{}.TurnCounterClockwise({})", *this, angle);
  ws_client_->send(Message::PerformTurn(token_, "COUNTER_CLOCKWISE", angle));
}

void Agent::Attack() const {
  spdlog::info("{}.Attack", *this);
  ws_client_->send(Message::PerformAttack(token_));
}

void Agent::UseSkill(SkillKind skill) const {
  spdlog::info("{}.UseSkill({})", *this, skill);
  ws_client_->send(Message::PerformSkill(token_, skill));
}

void Agent::SelectBuff(BuffKind buff) const {
  spdlog::info("{}.SelectBuff({})", *this, buff);
  ws_client_->send(Message::PerformSelect(token_, buff));
}

void Agent::Loop() const {
  if (!IsConnected()) {
    return;
  }
  ws_client_->send(Message::GetPlayerInfo(token_));
}

void Agent::OnMessage(std::string_view message) {
  try {
    if (auto msg_type{Message::ReadMessageType(message)};
        msg_type == "ALL_PLAYER_INFO") {
      Message::Read<"players">(players_info_, message);
    } else if (msg_type == "ENVIRONMENT_INFO") {
      Message::Read(environment_info_, message);
    } else if (msg_type == "GAME_STATISTICS") {
      Message::Read(game_statistics_, message);
    } else if (msg_type == "AVAILABLE_BUFFS") {
      Message::Read<"buffs">(available_buffs_, message);
    } else if (msg_type == "ERROR") {
      auto [error_code, error_message]{Message::ReadError(message)};
      spdlog::error("{} got an error from server: [{}] {}", *this, error_code,
                    error_message);
    }
  } catch (const std::exception& e) {
    spdlog::error("an error occurred in OnMessage({}): {}", *this, e.what());
#ifdef NDEBUG
    event_loop_->stop();
#endif
  }
}

}  // namespace thuai8_agent