#include <spdlog/spdlog.h>

#include "agent/agent.hpp"
#include "agent/available_buffs.hpp"
#include "agent/environment_info.hpp"
#include "agent/format.hpp"
#include "agent/player_info.hpp"

// NOLINTBEGIN(misc-use-internal-linkage)
void SelectBuff(thuai8_agent::Agent& agent) {
  const auto& self_info{agent.players_info()[0]};
  const auto& available_buffs{agent.available_buffs()};
  // Your code here
  agent.SelectBuff(thuai8_agent::BuffKind::Flash);
}

void Loop(thuai8_agent::Agent& agent) {
  const auto& self_info{agent.players_info()[0]};
  const auto& opponent_info{agent.players_info()[1]};
  const auto& walls{agent.environment_info().walls};
  const auto& fences{agent.environment_info().fences};
  const auto& bullets{agent.environment_info().bullets};
  const auto& game_statistics{agent.game_statistics()};
  // Your code here
  spdlog::info("{} move from {}",agent,self_info.position);
  agent.MoveForward();
  agent.MoveBackward();
  agent.TurnClockwise();
  agent.TurnCounterClockwise();
  agent.Attack();
  agent.UseSkill(thuai8_agent::SkillKind::Flash);
}
// NOLINTEND(misc-use-internal-linkage)
