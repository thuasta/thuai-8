#include "agent.hpp"

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>
#include <hv/hloop.h>
#include <spdlog/spdlog.h>

#include <cstdint>
#include <memory>
#include <string>
#include <string_view>

#include "available_buffs.hpp"
#include "message.hpp"
#include "player_info.hpp"

constexpr std::uint8_t kMinDelayMs{10};

namespace thuai8_agent {

Agent::Agent(std::string_view token, const hv::EventLoopPtr& event_loop,
             int loop_interval_ms)
    : token_(token), event_loop_(event_loop) {
  timer_id_ = event_loop->setInterval(loop_interval_ms,
                                      [this](hv::TimerID) { Loop(); });

  ws_client_ = std::make_unique<hv::WebSocketClient>(event_loop);

  reconn_setting_t reconn_setting;
  reconn_setting.delay_policy = 0;
  reconn_setting.min_delay = kMinDelayMs;
  ws_client_->setReconnect(&reconn_setting);

  ws_client_->onmessage = [this](std::string_view msg) { OnMessage(msg); };
}

void Agent::Connect(const std::string& server_address) {
  ws_client_->open(server_address.data());
}

void Agent::MoveForward() const {
  spdlog::debug("{}.MoveForward", *this);
  ws_client_->send(Message::PerformMove(token_, "FORTH"));
}

void Agent::MoveBackward() const {
  spdlog::debug("{}.MoveBackward", *this);
  ws_client_->send(Message::PerformMove(token_, "BACK"));
}

void Agent::TurnClockwise() const {
  spdlog::debug("{}.TurnClockwise", *this);
  ws_client_->send(Message::PerformTurn(token_, "CLOCKWISE"));
}

void Agent::TurnCounterClockwise() const {
  spdlog::debug("{}.TurnCounterClockwise", *this);
  ws_client_->send(Message::PerformTurn(token_, "COUNTER_CLOCKWISE"));
}

void Agent::Attack() const {
  spdlog::debug("{}.Attack", *this);
  ws_client_->send(Message::PerformAttack(token_));
}

void Agent::UseSkill(SkillKind skill) const {
  spdlog::debug("{}.UseSkill({})", *this, skill);
  ws_client_->send(Message::PerformSkill(token_, skill));
}

void Agent::SelectBuff(BuffKind buff) const {
  spdlog::debug("{}.SelectBuff({})", *this, buff);
  ws_client_->send(Message::PerformSelect(token_, buff));
}

void Agent::Loop() {
  if (!IsConnected()) {
    return;
  }
  ws_client_->send(Message::GetPlayerInfo(token_, "SELF"));
  ws_client_->send(Message::GetPlayerInfo(token_, "OPPONENT"));
  ws_client_->send(Message::GetGameStatistics(token_));
  ws_client_->send(Message::GetEnvironmentInfo(token_));
  ws_client_->send(Message::GetAvailableBuffs(token_));
}

void Agent::OnMessage(std::string_view message) {
  if (auto msg_type{Message::ReadMessageType(message)};
      msg_type == "PLAYER_INFO") {
    if (auto received_token{Message::ReadToken(message)};
        received_token == token_) {
      Message::ReadInfo(self_info_, message);
    } else {
      Message::ReadInfo(opponent_info_, message);
    }
  } else if (msg_type == "ENVIRONMENT_INFO") {
    Message::ReadInfo(environment_info_, message);
  } else if (msg_type == "GAME_STATISTICS") {
    Message::ReadInfo(game_statistics_, message);
  } else if (msg_type == "AVAILABLE_BUFFS") {
    Message::ReadInfo(available_buffs_, message);
  } else if (msg_type == "ERROR") {
    auto [error_code, error_message]{Message::ReadError(message)};
    spdlog::error("{} got an error from server: [{}] {}]", *this, error_code,
                  error_message);
  }
}

}  // namespace thuai8_agent
