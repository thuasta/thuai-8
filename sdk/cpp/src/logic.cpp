#include <spdlog/spdlog.h>

#include <algorithm>

#include "agent/agent.hpp"

namespace {
// NOLINTBEGIN(cppcoreguidelines-avoid-non-const-global-variables)
// Your global variables here
// But notice that you should avoid using global variables as much as possible
//
// NOLINTEND(cppcoreguidelines-avoid-non-const-global-variables)

//  Your functions here
//
}  // namespace

// NOLINTBEGIN(misc-use-internal-linkage)
void SelectBuff(const thuai8_agent::Agent& agent) {
  // Your code here
  // Here is an example of how to select a buff that is not in your skills
  const auto& self_info{agent.self_info()};
  const auto& available_buffs{agent.available_buffs()};

  spdlog::debug("AvailableBuffs: {}", available_buffs);
  for (auto buff : available_buffs) {
    if (std::ranges::none_of(self_info.skills, [buff](const auto& skill) {
          return buff == skill.name;
        })) {
      agent.SelectBuff(buff);
      return;
    }
  }
  agent.SelectBuff(available_buffs.front());
}

void Loop(const thuai8_agent::Agent& agent) {
  // Your code here
  // Here is an example of how to use the agent's functions
  const auto& self_info{agent.self_info()};
  const auto& opponent_info{agent.opponent_info()};

  spdlog::debug("Self {}, Opponent {}", self_info.position,
                opponent_info.position);

  agent.MoveForward();
  agent.MoveBackward();
  agent.TurnClockwise();
  agent.TurnCounterClockwise();

  if (self_info.weapon.currentBullets > 0) {
    agent.Attack();
  }

  for (const auto& skill : self_info.skills) {
    if (skill.currentCoolDown == 0) {
      agent.UseSkill(skill.name);
      break;
    }
  }
}
// NOLINTEND(misc-use-internal-linkage)