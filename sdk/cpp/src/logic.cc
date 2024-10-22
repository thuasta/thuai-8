#include <span>

#include "agent/agent.hpp"
#include "agent/available_buffs.hpp"
#include "agent/environment_info.hpp"

namespace thu = thuai8_agent;

// NOLINTBEGIN(misc-use-internal-linkage)
void Loop(thuai8_agent::Agent& agent) {
  const auto& self_info = agent.players_info()[0];
  const auto& opponent_info = agent.players_info()[1];
  const std::span<const thu::Wall> walls = agent.environment_info().walls;
  const std::span<const thu::Fence> fences = agent.environment_info().fences;
  const std::span<const thu::Bullet> bullets = agent.environment_info().bullets;
  const auto& game_statistics = agent.game_statistics();
  const std::span<const thu::BuffKind> available_buffs =
      agent.available_buffs();
  // Your code here
}
// NOLINTEND(misc-use-internal-linkage)
