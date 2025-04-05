#pragma once
#ifndef _THUAI8_AGENT_AGENT_HPP_
#define _THUAI8_AGENT_AGENT_HPP_

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>
#include <spdlog/fmt/bundled/format.h>

#include <memory>
#include <optional>
#include <string>
#include <string_view>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player.hpp"

namespace thuai8_agent {

class Agent {
 public:
  Agent(std::string_view token, const hv::EventLoopPtr& event_loop,
        int loop_interval_ms);

  Agent(const Agent&) = delete;
  Agent(Agent&&) = delete;
  auto operator=(const Agent&) -> Agent& = delete;
  auto operator=(Agent&&) -> Agent& = delete;

  ~Agent();

  void Connect(const std::string& server);

  [[nodiscard]] auto IsConnected() const -> bool {
    return ws_client_->isConnected();
  }

  [[nodiscard]] auto IsGameReady() const -> bool {
    return players_info_.has_value() && game_statistics_.has_value() &&
           environment_info_.has_value();
  }

  [[nodiscard]] auto token() const -> std::string_view { return token_; }

  [[nodiscard]] auto self_info() const -> const Player& {
    return players_info_.value().at(0).token == token_ ? players_info_->front()
                                                       : players_info_->at(1);
  }

  [[nodiscard]] auto opponent_info() const -> const Player& {
    return players_info_.value().at(0).token == token_ ? players_info_->at(1)
                                                       : players_info_->front();
  }

  [[nodiscard]] auto game_statistics() const -> const GameStatistics& {
    return game_statistics_.value();
  }

  [[nodiscard]] auto environment_info() const -> const EnvironmentInfo& {
    return environment_info_.value();
  }

  [[nodiscard]] auto available_buffs() const -> const AvailableBuffs& {
    return available_buffs_.value();
  }

  void MoveForward(float distance = MAX_MOVE_SPEED) const;

  void MoveBackward(float distance = MAX_MOVE_SPEED) const;

  void TurnClockwise(int angle = MAX_TURN_SPEED) const;

  void TurnCounterClockwise(int angle = MAX_TURN_SPEED) const;

  void Attack() const;

  void UseSkill(SkillKind skill) const;

  void SelectBuff(BuffKind buff) const;

  static constexpr float MAX_MOVE_SPEED{1.0F};
  static constexpr int MAX_TURN_SPEED{45};

 private:
  void Loop() const;
  void OnMessage(std::string_view message);

  const std::string token_;
  const hv::EventLoopPtr event_loop_;
  const std::unique_ptr<hv::WebSocketClient> ws_client_;
  const hv::TimerID timer_id_{};

  std::optional<Players> players_info_;
  std::optional<GameStatistics> game_statistics_;
  std::optional<EnvironmentInfo> environment_info_;
  std::optional<AvailableBuffs> available_buffs_;
};

}  // namespace thuai8_agent

template <>
struct fmt::formatter<thuai8_agent::Agent> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Agent& obj, format_context& ctx) {
    return fmt::format_to(ctx.out(), "Agent[{}]", obj.token());
  }
};

#endif  // _THUAI8_AGENT_AGENT_HPP_