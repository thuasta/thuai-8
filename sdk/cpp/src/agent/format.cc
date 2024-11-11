#include "format.hpp"

#include <fmt/format.h>
#include <fmt/ranges.h>

#include <magic_enum.hpp>
#include <string>

#include "agent.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player_info.hpp"
#include "position.hpp"

namespace thuai8_agent {

auto format_as(const Agent& object) -> std::string {
  return fmt::format("Agent[Token: {}]", object.token());
}

auto format_as(const Position& object) -> std::string {
  return fmt::format("Position: [x: {}, y: {}, angle: {}]", object.x, object.y,
                     object.angle);
}

auto format_as(const Weapon& object) -> std::string {
  return fmt::format(
      "Weapon: [IsLaser: {},  AntiArmor: {}, Damage: {}, MaxBullets: {}, "
      "CurrentBullets: {}, AttackSpeed: {}, BulletSpeed: {}]",
      object.isLaser, object.antiArmor, object.damage, object.maxBullets,
      object.currentBullets, object.attackSpeed, object.bulletSpeed);
}

auto format_as(const Armor& object) -> std::string {
  return fmt::format(
      "Armor: [CanReflect: {}, GravityField: {}, ArmorValue: {}, Health: {}, "
      "DodgeRate: {}, Knife: {}]",
      object.canReflect, object.gravityField, object.armorValue, object.health,
      object.dodgeRate, object.knife);
}

auto format_as(const Skill& object) -> std::string {
  return fmt::format(
      "Skill[Name: {}, MaxCoolDown: {}, CurrentCoolDown: {}, IsActive: {}]",
      object.name, object.maxCoolDown, object.currentCoolDown, object.isActive);
}

auto format_as(const PlayerInfo& object) -> std::string {
  return fmt::format("PlayerInfo[Token: {}, {}, {}, {}, Skills: {{{}}}]",
                     object.token, object.position, object.weapon, object.armor,
                     object.skills);
}

auto format_as(const Wall& object) -> std::string {
  return fmt::format("Wall[{}]", object.position);
}

auto format_as(const Fence& object) -> std::string {
  return fmt::format("Fence[{}, health: {}]", object.position, object.health);
}

auto format_as(const Bullet& object) -> std::string {
  return fmt::format("Bullet[{}, speed: {}, damage: {}, traveledDistance: {}]",
                     object.position, object.speed, object.damage,
                     object.traveledDistance);
}

auto format_as(const EnvironmentInfo& object) -> std::string {
  return fmt::format(
      "EnvironmentInfo[Walls: {{{}}}, Fences: {{{}}}, Bullets: {{{}}}]",
      object.walls, object.fences, object.bullets);
}

auto format_as(const OnesScore& object) -> std::string {
  return fmt::format("Token {} : Score {}", object.token, object.score);
}

auto format_as(const GameStatistics& object) -> std::string {
  return fmt::format(
      "GameStatistics[Stage: {}, CountDown: {}, Ticks: {}, Scores: ({} : {} = "
      "{} : {})]",
      object.currentStage, object.countDown, object.ticks,
      object.scores[0].token, object.scores[1].token, object.scores[0].score,
      object.scores[1].score);
}

}  // namespace thuai8_agent