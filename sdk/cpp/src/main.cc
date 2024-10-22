#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <spdlog/spdlog.h>

#include <cstdlib>
#include <cxxopts.hpp>
#include <exception>
#include <memory>
#include <optional>
#include <print>
#include <string>
#include <string_view>
#include <utility>

#include "agent/agent.hpp"
#include "agent/format.hpp"

extern void Loop(thuai8_agent::Agent& agent);

constexpr auto kDefaultServer{std::string_view{"ws://localhost:14514"}};
constexpr auto kDefaultToken{std::string_view{"1919810"}};
constexpr auto kDefaultIntervalMs{200};

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

  cxxopts::Options options{"agent"};
  options.add_options()("s,server", "Server address",
                        cxxopts::value<std::string>(server))(
      "t,token", "Token", cxxopts::value<std::string>(token))("h,help",
                                                              "Print usage");

  auto result{options.parse(argc, argv)};

  if (result.count("help") > 0) {
    std::println("{}", options.help());
    return std::nullopt;
  }

  server = result["server"].as<std::string>();
  token = result["token"].as<std::string>();

  return {std::make_pair(server, token)};
}
}  // namespace

auto main(int argc, char* argv[]) -> int {
  auto options{ParseOptions(argc, argv)};
  if (!options.has_value()) {
    return 0;
  }
  auto [server, token]{options.value()};

  hv::EventLoopPtr event_loop{std::make_shared<hv::EventLoop>()};

  thuai8_agent::Agent agent{token, event_loop, kDefaultIntervalMs};

  spdlog::info("{} is starting with server {}", agent, server);

  event_loop->runInLoop([&] { agent.Connect(server); });

  bool is_previous_connected{false};
  bool is_previous_game_ready{false};

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
      spdlog::debug("{} is waiting for game to be ready", agent);
      return;
    }

    if (!is_previous_game_ready) {
      spdlog::info("{} is in a ready game", agent);
      is_previous_game_ready = true;
    }

    try {
      Loop(agent);
    } catch (const std::exception& e) {
      spdlog::error("an error occurred in loop({}): {}", agent, e.what());
    }
  });

  event_loop->run();
}
