#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <spdlog/spdlog.h>

#ifndef NDEBUG
#include <chrono>
#endif
#include <cstdint>
#include <cstdlib>
#include <cxxopts.hpp>
#include <exception>
#include <memory>
#include <optional>
#include <print>
#include <string>
#include <utility>

#include "agent/agent.hpp"
#include "agent/format.hpp"
#include "agent/game_statistics.hpp"

extern void SelectBuff(thuai8_agent::Agent& agent);
extern void Loop(thuai8_agent::Agent& agent);

constexpr auto kDefaultServer{"ws://localhost:14514"};
constexpr auto kDefaultToken{"1919810"};
constexpr std::uint8_t kDefaultIntervalMs{200};

namespace {
auto ParseOptions(int argc, char** argv)
    -> std::optional<std::pair<std::string, std::string>> {
  std::string server{kDefaultServer};
  std::string token{kDefaultToken};

  // NOLINTBEGIN(concurrency-mt-unsafe)
  if (auto* env_server{std::getenv("SERVER")}; env_server != nullptr) {
    server = env_server;
  }
  if (auto* env_token{std::getenv("TOKEN")}; env_token != nullptr) {
    token = env_token;
  }
  // NOLINTEND(concurrency-mt-unsafe)

  if (argc > 1) {
    cxxopts::Options options{"agent"};
    options.add_options()("s,server", "Set server_address",
                          cxxopts::value<std::string>()->default_value(server))(
        "t,token", "Set token",
        cxxopts::value<std::string>()->default_value(token))("h,help",
                                                             "Print usage");
    options.allow_unrecognised_options();

    auto result{options.parse(argc, argv)};

    if (result.unmatched().size() > 0) {
      std::print("\033[1;31mUnrecognized options: {}\033[0m",
                 result.unmatched().front());
      std::println("{}", options.help());
      return std::nullopt;
    }
    if (result.count("help") > 0) {
      std::println("{}", options.help());
      return std::nullopt;
    }

    server = result["server"].as<std::string>();
    token = result["token"].as<std::string>();
  }

  return std::make_pair(server, token);
}
}  // namespace

auto main(int argc, char* argv[]) -> int {
#ifndef NDEBUG
  spdlog::set_level(spdlog::level::debug);
#endif

  auto options{ParseOptions(argc, argv)};
  if (!options.has_value()) {
    return 0;
  }
  auto [server, token]{options.value()};

  hv::EventLoopPtr event_loop{std::make_unique<hv::EventLoop>()};

  thuai8_agent::Agent agent{token, event_loop, kDefaultIntervalMs};

  spdlog::info("{} is starting with server {}", agent, server);

  event_loop->runInLoop([&] { agent.Connect(server); });

  bool is_previous_connected{false};
  bool is_previous_game_ready{false};
  bool is_buff_selected{false};

  event_loop->setInterval(kDefaultIntervalMs, [&](hv::TimerID) {
    if (!agent.IsConnected()) {
      if (is_previous_connected) {
        spdlog::error("{} disconnected from server", agent);
        is_previous_connected = false;
      }
      spdlog::debug("{} is waiting for connection", agent);
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
      spdlog::debug("{} is waiting for the game to be ready", agent);
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
          spdlog::info("{} selected a buff", agent);
          is_buff_selected = true;
          return;
        } catch (const std::exception& e) {
          spdlog::error("an error occurred in SelectBuff({}): {}", agent,
                        e.what());
#ifdef NDEBUG
          event_loop->stop();
#endif
        }
      }
      spdlog::debug("{} is waiting for next battle", agent);
      return;
    }

    if (is_buff_selected) {
      is_buff_selected = false;
    }

    try {
#ifndef NDEBUG
      auto start = std::chrono::high_resolution_clock::now();
#endif
      Loop(agent);
#ifndef NDEBUG
      if (auto end = std::chrono::high_resolution_clock::now();
          std::chrono::duration_cast<std::chrono::milliseconds>(end - start)
              .count() > kDefaultIntervalMs) {
        spdlog::warn("{} PlayerLoop overflow {}ms", agent, kDefaultIntervalMs);
      }
#endif
    } catch (const std::exception& e) {
      spdlog::error("an error occurred in PlayerLoop({}): {}", agent, e.what());
#ifdef NDEBUG
      event_loop->stop();
#endif
    }
  });

  try {
    event_loop->run();
  } catch (const std::exception& e) {
    spdlog::error("an error occurred in EventLoop({}): {}", agent, e.what());
  }
}
