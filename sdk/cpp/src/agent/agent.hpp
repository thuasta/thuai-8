#pragma once
#ifndef _THUAI8_AGENT_AGENT_HPP_
#define _THUAI8_AGENT_AGENT_HPP_

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>

#include <memory>
#include <optional>
#include <string>
#include <string_view>
#include <vector>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "message.hpp"
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

  [[nodiscard]] auto IsConnected() -> bool;

  [[nodiscard]] auto IsGameReady() -> bool;

  [[nodiscard]] auto token() const -> std::string_view;

  [[nodiscard]] auto self_info() const -> const PlayerInfo&;

  [[nodiscard]] auto opponent_info() const -> const PlayerInfo&;

  [[nodiscard]] auto game_statistics() const -> const GameStatistics&;

  [[nodiscard]] auto environment_info() const -> const EnvironmentInfo&;

  [[nodiscard]] auto available_buffs() const -> const std::vector<BuffKind>&;

  void MoveForward() const;

  void MoveBackward() const;

  void TurnClockwise() const;

  void TurnCounterClockwise() const;

  void Attack() const;

  void UseSkill(SkillKind skill) const;

  void SelectBuff(BuffKind buff) const;

 private:
  void Loop();
  void OnMessage(const Message& message);

  std::string token_;
  hv::EventLoopPtr event_loop_;
  hv::TimerID timer_id_;
  std::unique_ptr<hv::WebSocketClient> ws_client_;

  std::optional<PlayerInfo> self_info_;
  std::optional<PlayerInfo> opponent_info_;
  std::optional<GameStatistics> game_statistics_;
  std::optional<EnvironmentInfo> environment_info_;
  std::optional<std::vector<BuffKind>> available_buffs_;
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_AGENT_HPP_