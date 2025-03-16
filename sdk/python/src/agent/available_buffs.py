from typing import List
from .buffname import BuffName


class AvailableBuffs:
    def __init__(self, buffs: List[BuffName]):
        self.buffs = buffs

    def __str__(self):
        buffs_str = ", ".join(str(buff) for buff in self.buffs)
        return f"AvailableBuffs(buffs=[{buffs_str}])"
