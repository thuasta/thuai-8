# THUAI8 Agent Template (C++)

C++ agent template for the 8th Tsinghua University Artificial Intelligence Challenge.

## Usage

### Prerequisites

- [XMake](https://xmake.io/#/zh-cn/) >= 2.9.8
- C++ compiler toolchain with C++23 support

### Build

Run the following code to configure the project:

```bash
xmake f -m debug
```

Or in release mode:

```bash
xmake f -m release
```

Then build the project:

```bash
xmake
```

### Write Your Code

You can write your code in `logic.cpp`. For experienced developers, you can also modify any other files in the project.

### Run

If you modify the code in `main.cpp`, this section may be invalid.

Run the following command to start the agent:

```bash
./agent --server <server> --token <token>
```

- `<server>`: The server address of the game. (Default: `ws://localhost:14514`)
- `<token>`: The token of the agent. (Default: `1919810`)

For example:

```bash
./agent --server ws://localhost:14514 --token 1919810
```

Or pass the arguments with environment variables.

## Contributing

Ask questions by creating an issue.

PRs accepted.

## License

CC0-1.0 Public Domain Dedication
