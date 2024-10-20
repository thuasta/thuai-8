#pragma once
#ifndef _THUAI8_AGENT_AGENT_HPP_
#define _THUAI8_AGENT_AGENT_HPP_

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>

#include <functional>
#include <memory>
#include <optional>
#include <span>
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

  ~Agent();

  // Methods for interacting with the game server

  void Connect(const std::string& server_address);

  void Disconnect();

  [[nodiscard]] auto IsConnected() const -> bool;

  [[nodiscard]] auto token() const -> std::string;

  // Methods for interacting with the game

  [[nodiscard]] auto IsGameReady() const -> bool;

  [[nodiscard]] auto players_info() const
      -> std::optional<std::span<const PlayerInfo>>;

  [[nodiscard]] auto game_statistics() const
      -> std::optional<std::reference_wrapper<const GameStatistics>>;

  [[nodiscard]] auto environment_info() const
      -> std::optional<std::reference_wrapper<const EnvironmentInfo>>;

  [[nodiscard]] auto available_buffs() const
      -> std::optional<std::span<const BuffKind>>;

  void MoveForward();

  void MoveBackward();

  void TurnClockwise();

  void TurnCounterClockwise();

  void Attack();

  void UseSkill(SkillKind skill);

  void SelectBuff(BuffKind buff);

 private:
  void Loop();
  void OnMessage(const Message& message);

  std::string token_;
  hv::EventLoopPtr event_loop_;
  hv::TimerID timer_id_;
  std::unique_ptr<hv::WebSocketClient> ws_client_;

  std::optional<std::vector<PlayerInfo>> players_;
  std::optional<GameStatistics> game_statistics_;
  std::optional<EnvironmentInfo> environment_info_;
  std::optional<std::vector<BuffKind>> available_buffs_;
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_AGENT_HPP_