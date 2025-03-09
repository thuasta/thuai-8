FROM python:3.11.11-slim-bookworm AS build-env
WORKDIR /app
COPY requirements.txt .
RUN pip install --disable-pip-version-check --no-cache-dir -r requirements.txt

FROM gcr.io/distroless/python3-debian12
WORKDIR /app
COPY --from=build-env /usr/local/lib/python3.11/site-packages /usr/local/lib/python3.11/site-packages
COPY . .
ENV PYTHONPATH=/usr/local/lib/python3.11/site-packages
ENTRYPOINT ["python", "src/main.py"]
