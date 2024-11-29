#pragma once
#ifndef _THUAI8_ENVIRONMENT_INFO_HPP_
#define _THUAI8_ENVIRONMENT_INFO_HPP_

#include <vector>

#include "agent/position.hpp"

namespace thuai8_agent {

struct Wall {
  Position<int> position{};
};

struct Fence {
  Position<int> position{};
  unsigned int health{};
};

struct Bullet {
  Position<double> position{};
  double speed{};
  double damage{};
  double traveledDistance{};  // 子弹已经过路程
};

struct Laser {
  Position<double> start{};
  Position<double> end{};
};

struct EnvironmentInfo {
  unsigned int mapSize{};
  std::vector<Wall> walls;
  std::vector<Fence> fences;
  std::vector<Bullet> bullets;
};

}  // namespace thuai8_agent

#endif  // _THUAI8_ENVIRONMENT_INFO_HPP_