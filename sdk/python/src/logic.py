import logging

from agent.agent import Agent


async def selectBuff(agent: Agent):
    # Your code here.
    # Here is an example of how to select a buff.
    # Always select the first buff in the available buff list.
    available_buffs = agent.availiable_buffs
    assert available_buffs is not None

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

    await agent.move_forward()
    await agent.move_backward()
    await agent.turn_clockwise()
    await agent.turn_counter_clockwise()

    if self_info.weapon.current_bullets > 0:
        await agent.attack()

    for skill in self_info.skills:
        if skill.current_cooldown == 0:
            await agent.use_skill(skill)
            break
