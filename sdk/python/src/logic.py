import logging
from typing import List, Optional

from pathfinding.core.grid import Grid
from pathfinding.finder.best_first import BestFirst

from agent.agent import Agent
from agent.position import Position
from agent.enviroment_info import Wall, Fence, Bullet

wall_list: Optional[List[Wall]] = None
fence_list: Optional[List[Fence]] = None
bullets_list: Optional[List[Bullet]] = None


async def setup(agent: Agent):
    # Your code here.
    pass


async def loop(agent: Agent):
    # Your code here.
    # Here is an example of how to use the agent.
    # Always move to the opponent's position, keep a proper distance away from the
    # opponent, and attack the opponent.

    player_info_list = agent.all_player_info
    assert player_info_list is not None

    self_id = agent.self_id
    assert self_id is not None

    self_info = player_info_list[self_id]
    opponent_info = player_info_list[(self_id + 1) % len(player_info_list)]

    environment_info = agent.environment_info
    assert environment_info is not None

    # Record the positions of walls and bullets in the environment
    global wall_list
    global fence_list
    global bullets_list

    wall_list = environment_info.walls
    fence_list = environment_info.fences
    bullets_list = environment_info.bullets

    self_position_float = self_info.position
    opponent_position_float = opponent_info.position

    # Record the optimal path
    global path

    path = find_path(self_position_float, opponent_position_float, wall_list, fence_list, bullets_list)

    if len(path) == 0:
        logging.info(
            "no path from %s to %s", self_position_float, opponent_position_float
        )
        return

    logging.info(f"found path from {self_position_float} to {opponent_position_float}")

    if len(path) > 1:
        await agent.turn(path[0][0])
        await agent.move(path[0][1])
        return

   # If there's only one step, attack directly
    await agent.turn(path[0][0])
    await agent.attack()


def find_path(start: Position, end: Position, walls: List[Wall], fences: List[Fence], bullets: List[Bullet]) -> List[(float, float)]:
    # Your code here.
    # Design a pathfinding algorithm based on the positions of walls and bullets in the environment
    # Return a list of the optimal path from start to end, each element being a tuple (angle, distance)
    pass



