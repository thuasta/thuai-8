FROM gcc:13.3.0-bookworm AS build-env
WORKDIR /app
RUN curl -fsSL https://xmake.io/shget.text | bash
COPY xmake.lua .
RUN ~/.local/bin/xmake f -m release -v -y --root
COPY . .
RUN ~/.local/bin/xmake -v --root

FROM ubuntu:24.04
WORKDIR /app
COPY --from=build-env /app/bin/agent .
ENTRYPOINT ["./agent"]
