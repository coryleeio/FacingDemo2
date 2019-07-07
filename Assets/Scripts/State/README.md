State holding objects and enums that dont have their own folder like entities, items, etc.

Anything json ignored is not saved when the game is saved.  Json ignored properties are typically 'Derived' from other stateful values.

Typically these should not implement any game logic, though sometimes they do.  When they do they'll typically delegate to a system or utility class in 'Systems'
