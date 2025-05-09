import logging
import math

from agent.agent import Agent


async def selectBuff(agent: Agent):
    available_buffs = agent.availiable_buffs
    if (available_buffs is None) or (len(available_buffs.buffs) == 0):
        logging.warning("No available buffs")
        return

    # Your code here.
    # Here is an example of how to select a buff.
    # Always select the first buff in the available buff list.
    await agent.select_buff(available_buffs.buffs[0].name)


async def loop(agent: Agent):
    # Your code here.
    # Here is an example of how to use the agent.
    player_info_list = agent.all_player_info
    assert player_info_list is not None

    self_info = None
    opponent_info = None

    for player in player_info_list:
        if player.token == agent.token:
            self_info = player
        else:
            opponent_info = player

    logging.debug(f"Self: {self_info.position}, Opponent: {opponent_info.position}")

    environment_info = agent.environment_info
    assert environment_info is not None
    walls = environment_info.walls
    bullets = environment_info.bullets

    px = self_info.position.x
    py = self_info.position.y
    player_angle = self_info.position.angle

    is_safe = True

    wall_safe_distance = 1.0
    for wall in walls:
        wall_pos = wall.position
        wx = wall_pos.x
        wy = wall_pos.y
        wall_angle = wall_pos.angle

        if wall_angle == 0:
            distance = wy - py
            if abs(distance) < wall_safe_distance:
                is_safe = False
                if math.tan(player_angle) > 0:
                    await agent.turn_clockwise()
                else:
                    await agent.turn_counter_clockwise()

        elif wall_angle == 90:
            distance = wx - px
            if abs(distance) < wall_safe_distance:
                is_safe = False
                if math.tan(player_angle) > 0:
                    await agent.turn_counter_clockwise()
                else:
                    await agent.turn_clockwise()
        else:
            continue

    bullet_danger_distance = 3.0
    for bullet in bullets:
        bx = bullet.position.x
        by = bullet.position.y
        distance = math.hypot(px - bx, py - by)
        if distance < bullet_danger_distance:
            is_safe = False
            if math.tan(player_angle) > math.tan(bullet.position.angle):
                await agent.turn_clockwise()
            else:
                await agent.turn_counter_clockwise()
            await agent.move_forward()
            break

    if is_safe and self_info.weapon.current_bullets > 0:
        await agent.attack()

    for skill in self_info.skills:
        if skill.current_cooldown == 0:
            await agent.use_skill(skill.name)
            break
