#pragma once
#ifndef _THUAI8_AGENT_AGENT_HPP_
#define _THUAI8_AGENT_AGENT_HPP_

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>

#include <format>
#include <memory>
#include <optional>
#include <string>
#include <string_view>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player_info.hpp"

namespace thuai8_agent {

class Agent {
 public:
  Agent(std::string_view token, const hv::EventLoopPtr& event_loop,
        int loop_interval_ms);

  Agent(const Agent&) = delete;
  Agent(Agent&&) = delete;
  auto operator=(const Agent&) -> Agent& = delete;
  auto operator=(Agent&&) -> Agent& = delete;
  ~Agent() = default;

  void Connect(const std::string& server_address);

  [[nodiscard]] auto IsConnected() const -> bool {
    return ws_client_->isConnected();
  }

  [[nodiscard]] auto IsGameReady() const -> bool {
    return self_info_.has_value() && opponent_info_.has_value() &&
           game_statistics_.has_value() && environment_info_.has_value() &&
           available_buffs_.has_value();
  }

  [[nodiscard]] auto token() const -> std::string_view { return token_; }

  [[nodiscard]] auto self_info() const -> const PlayerInfo& {
    return self_info_.value();
  }

  [[nodiscard]] auto opponent_info() const -> const PlayerInfo& {
    return opponent_info_.value();
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

  void MoveForward() const;

  void MoveBackward() const;

  void TurnClockwise() const;

  void TurnCounterClockwise() const;

  void Attack() const;

  void UseSkill(SkillKind skill) const;

  void SelectBuff(BuffKind buff) const;

 private:
  void Loop();
  void OnMessage(std::string_view message);

  std::string token_;
  hv::EventLoopPtr event_loop_;
  hv::TimerID timer_id_;
  std::unique_ptr<hv::WebSocketClient> ws_client_;

  std::optional<PlayerInfo> self_info_;
  std::optional<PlayerInfo> opponent_info_;
  std::optional<GameStatistics> game_statistics_;
  std::optional<EnvironmentInfo> environment_info_;
  std::optional<AvailableBuffs> available_buffs_;
};

}  // namespace thuai8_agent

template <>
struct std::formatter<thuai8_agent::Agent> : std::formatter<string> {
  template <class FormatContext>
  auto format(const thuai8_agent::Agent& object, FormatContext& ctx) const {
    return format_to(ctx.out(), "Agent[Token: {}]", object.token());
  }
};

#endif  // _THUAI8_AGENT_AGENT_HPP_