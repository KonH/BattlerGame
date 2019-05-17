# Battler Game

## Summary

The client-server game with battler-like mechanics

## Features

- Turn-based battle system
- Linear level progression
- Units & items with different properties
- Event levels
- Time-limited (farming) levels
- Daily rewards

## Tech

- Client-server communication, the authoritative server
- JSON for data transfer, state & config
- JWT for auth
- Command pattern to hold logic into short classes
- Game state modified on client & server, it works deterministically, full state transition isn't required after login
- Client-side cheats are not allowed, server applying only valid commands
- Server: works in memory, provide auth, state storage, and command validation
- Unity client: support all required commands and have the ability to simulate server locally
- Console client: support all available commands using reflection
- Unit Tests: cover almost all use cases, related to commands
- Async tweening extensions

## Tools

- .NET Core 3.0
- Unity 2019.1
