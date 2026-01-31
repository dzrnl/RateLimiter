# Rate Limiter

Event‑driven система контроля RPM для сервисов. Проект разделяет управление лимитами (control plane) и применение
лимитов (data plane)

## Архитектура и компоненты

- **UserService (gRPC)**  
  CRUD пользователей в PostgreSQL (Dapper)
  Interceptor‑ы:
    - обязательный заголовок `user_id`;
    - проверка блокировки в Redis;
    - единая обработка ошибок.

  Есть in‑memory кэш пользователей (по id и по name+surname).   
  Внедрен подход с доменными интерфейсами: request/Db‑модели (через partial) их реализуют, что убирает лишний маппинг и
  модели

- **RateLimiter.Writer (gRPC)**  
  Управление лимитами по маршрутам (create/get/update/delete).  
  Хранение в MongoDB

- **RateLimiter.Reader (gRPC + Kafka)**  
  При старте загружает лимиты из Mongo батчами и держит их в памяти.  
  Через Mongo Change Streams поддерживает кэш в актуальном состоянии.  
  Потребляет события запросов из Kafka и применяет RPM‑логику

- **UserRequestsKafkaGenerator (HTTP + Kafka)**  
  Планировщик генерации событий запросов в Kafka.  
  Позволяет динамически добавлять/обновлять расписания во время работы

## Поток данных

1. **Writer** создает/обновляет лимиты в MongoDB
2. **Reader** загружает лимиты в память и слушает изменения
3. **Generator** шлет в Kafka события `{ user_id, endpoint }`
4. **Reader** считает запросы в Redis и ставит блокировки
5. **UserService** читает блокировки в Redis и пропускает/отклоняет запросы

## Алгоритм rate limiting

- Для каждого `(userId, endpoint)` в Redis ведется счетчик с TTL = 1 минута
- Если счетчик превышает лимит RPM — выставляется флаг блокировки с TTL (по умолчанию 5 минут)
- UserService проверяет этот флаг перед обработкой запроса

## Запуск

```bash
docker compose up --build
```

## Порты

- **UserService (gRPC):** `localhost:5002`
- **RateLimiter.Writer (gRPC):** `localhost:5001`
- **RateLimiter.Reader (gRPC):** `localhost:4999`
- **Kafka Generator (HTTP):** `localhost:4998`
- **Postgres:** `localhost:5432`
- **MongoDB:** `localhost:5433`
- **Redis:** `localhost:6379`
- **RedisInsight:** `localhost:5540`
- **Kafka:** `localhost:9092`
- **Kafka Console:** `localhost:8079`

## Тесты

```bash
dotnet test UserService/UserService.csproj
```
