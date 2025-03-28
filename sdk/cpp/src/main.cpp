#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <spdlog/spdlog.h>

#ifndef NDEBUG
#include <chrono>
#endif
#include <cstdlib>
#include <cxxopts.hpp>
#include <exception>
#include <iostream>
#include <memory>
#include <optional>
#include <string>
#include <utility>

#include "agent/agent.hpp"
#include "agent/game_statistics.hpp"

extern void SelectBuff(const thuai8_agent::Agent& agent);
extern void Loop(const thuai8_agent::Agent& agent);

constexpr auto kDefaultServer{"ws://localhost:14514"};
constexpr auto kDefaultToken{"1919810"};
constexpr int kDefaultIntervalMs{200};

namespace {
#ifndef NDEBUG
void SetLogLevel(const std::string& level) {
  if (level == "0" || level == "trace") {
    spdlog::set_level(spdlog::level::trace);
    return;
  }
  if (level == "1" || level == "debug") {
    spdlog::set_level(spdlog::level::debug);
    return;
  }
  if (level == "2" || level == "info") {
    spdlog::set_level(spdlog::level::info);
    return;
  }
  if (level == "3" || level == "warn") {
    spdlog::set_level(spdlog::level::warn);
    return;
  }
  if (level == "4" || level == "error") {
    spdlog::set_level(spdlog::level::err);
    return;
  }
  throw std::invalid_argument("Invalid log level '" + level + "'");
}
#endif

auto ParseOptions(int argc, char** argv)
    -> std::optional<std::pair<std::string, std::string>> {
  std::string server{kDefaultServer};
  std::string token{kDefaultToken};

  // NOLINTBEGIN(concurrency-mt-unsafe)
  if (auto* env_server{std::getenv("GAME_HOST")}) {
    server = env_server;
  }
  if (auto* env_token{std::getenv("TOKEN")}) {
    token = env_token;
  }
  // NOLINTEND(concurrency-mt-unsafe)

  if (argc > 1) {
    cxxopts::Options options{"agent"};
    options.add_options()("h,help", "Print usage")(
        "s,server", "Set server_address",
        cxxopts::value<std::string>()->default_value(server))(
        "t,token", "Set token",
        cxxopts::value<std::string>()->default_value(token));
#ifndef NDEBUG
    options.add_options()(
        "l,log", "Set log level: 0=trace, 1=debug, 2=info, 3=warn, 4=error",
        cxxopts::value<std::string>()->default_value("0"));
#endif

    try {
      auto result{options.parse(argc, argv)};
      if (result.count("help") > 0) {
        std::cout << options.help() << '\n';
        return std::nullopt;
      }
      server = result["server"].as<std::string>();
      token = result["token"].as<std::string>();
#ifndef NDEBUG
      SetLogLevel(result["log"].as<std::string>());
#endif
    } catch (const std::exception& e) {
      std::cout << "\033[1;31m" << e.what() << "\033[0m";
      std::cout << options.help() << '\n';
      return std::nullopt;
    }
  }
#ifndef NDEBUG
  else {
    spdlog::set_level(spdlog::level::trace);
  }
#endif
  spdlog::set_pattern("[%H:%M:%S.%e] [%^%l%$] %v");

  return std::make_pair(server, token);
}
}  // namespace

// NOLINTBEGIN(readability-function-cognitive-complexity)
auto main(int argc, char* argv[]) -> int {
  auto options{ParseOptions(argc, argv)};
  if (!options.has_value()) {
    return 0;
  }

  hv::EventLoopPtr event_loop{std::make_shared<hv::EventLoop>()};

  thuai8_agent::Agent agent{options->second, event_loop, kDefaultIntervalMs};

  event_loop->runInLoop([&] { agent.Connect(options->first); });

  spdlog::info("{} is starting with server {}", agent, options->first);

  bool is_previous_connected{false};
  bool is_previous_game_ready{false};
  bool is_buff_selected{false};

  event_loop->setInterval(kDefaultIntervalMs, [&](hv::TimerID) {
    if (!agent.IsConnected()) {
      if (is_previous_connected) {
        spdlog::error("{} disconnected from server", agent);
        is_previous_connected = false;
      }
      spdlog::trace("{} is waiting for connection", agent);
      return;
    }

    if (!is_previous_connected) {
      spdlog::info("{} is connected to server", agent);
      is_previous_connected = true;
    }

    if (!agent.IsGameReady()) {
      if (is_previous_game_ready) {
        spdlog::error("{} is no longer in a ready game", agent);
        is_previous_game_ready = false;
      }
      spdlog::trace("{} is waiting for the game to be ready", agent);
      return;
    }

    if (!is_previous_game_ready) {
      spdlog::info("{} is in a ready game", agent);
      is_previous_game_ready = true;
    }

    if (agent.game_statistics().currentStage == thuai8_agent::Stage::Rest) {
      if (!is_buff_selected) {
        try {
          SelectBuff(agent);
          is_buff_selected = true;
        } catch (const std::exception& e) {
          spdlog::error("an error occurred in SelectBuff({}): {}", agent,
                        e.what());
#ifdef NDEBUG
          event_loop->stop();
#endif
        }
      }
      spdlog::trace("{} is waiting for next battle", agent);
      return;
    }

    if (is_buff_selected) {
      spdlog::info("{} is in a new battle", agent);
      is_buff_selected = false;
    }

#ifndef NDEBUG
    auto start{std::chrono::high_resolution_clock::now()};
#endif
    try {
      Loop(agent);
    } catch (const std::exception& e) {
      spdlog::error("an error occurred in PlayerLoop({}): {}", agent, e.what());
#ifdef NDEBUG
      event_loop->stop();
#endif
    }
#ifndef NDEBUG
    auto end{std::chrono::high_resolution_clock::now()};
    if (auto duration{
            std::chrono::duration_cast<std::chrono::milliseconds>(end - start)
                .count()};
        duration > kDefaultIntervalMs) {
      spdlog::warn("PlayerLoop({}) overflow {}ms with {}ms", agent,
                   kDefaultIntervalMs, duration);
    }
#endif
  });

  event_loop->run();
}
// NOLINTEND(readability-function-cognitive-complexity)