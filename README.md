# Uala Challenge

## Arquitetura Implementada
![Arquitectura de la solucion](./docs/arquitectura.png)

### Vision General
La arquitectura está diseñada como un sistema distribuido basado en microservicios, orientado a la escalabilidad. Los usuarios acceden a la aplicación a través de interfaces web y/o móvil, y las solicitudes son gestionadas por una serie de capas antes de llegar a los servicios de backend y las bases de datos.

#### Componentes Principales

##### API Gateway
Actúa como el punto de entrada único para todas las solicitudes de los clientes.
Se encarga del enrutamiento de las solicitudes, la limitación de velocidad (rate limiting).
El framework utilizando fue ocelot.

#### Consumers
Son los encargados de la comunicacion http a los servicios en gRPC.

#### Microservicios
La lógica de negocio principal está dividida en varios microservicios con gPRC. Cada uno de ellos es responsable de una funcionalidad específica.

#### Cache (Redis)
Se utiliza una caché para almacenar datos de acceso frecuente, mejorando el rendimiento y reduciendo la carga sobre los servicios y bases de datos principales.

#### Message Brokers (RabbitMQ)
Se utiliza un message broker para facilitar la comunicación asíncrona entre microservicios.
Permiten desacoplar los servicios, de modo que el productor de un mensaje no necesita esperar a que el consumidor lo procese. Esto mejora la escalabilidad del sistema.

#### Bases de Datos
Se utiliza una base de datos relacional (Oracle) para la persistencia de las entidades (Tweets y Timeline) ya que nos brinda rendimiento, escalabilidad, consistencia y biabilidad de datos.

Para la entidad "Follow" se opto por una base de datos no relacional (MongoDB) por su esquema flexible, escalabilidad horizontal, rendimiento de carga y alta disponibilidad.

### Flujo de datos (General)
1. El usuario realiza una solicitud desde un cliente web o móvil.
La solicitud llega al Balanceador de Carga (No implementado).

2. El Balanceador de Carga redirige la solicitud a una instancia del API Gateway.

3. El API Gateway enruta al microservicio apropiado.

4. Los microservicios procesan la solicitud.

5. Los "Workers"  consumen los mensajes del Message Broker,  procesan los datos y los persisten en la base de datos correspondiente.

## Contenedores
Para ejecutar la aplicacion es necesario tenes instalado docker y ejecutar `[docker-compose](./docs/docker-compose.yml) o bien copiar y ejecutar el siguiente script
```
version: '3.8'

services:
  rabbitmq:
    image: rabbitmq:3-management-alpine
    container_name: rabbitmq_server
    hostname: my-rabbit
    ports:
      - "5672:5672"
      - "15672:15672"
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq/
    environment:
      RABBITMQ_DEFAULT_USER: guest 
      RABBITMQ_DEFAULT_PASS: guest
    networks:
      - uala_net

  redis:
    image: redis:latest
    container_name: my-redis-cache
    restart: always
    ports:
      - "6379:6379"
    volumes:
      - redis_data:/data
      - ./users.acl:/usr/local/etc/redis/users.acl
    networks:
      - uala_net
    healthcheck:
      test: ["CMD", "redis-cli", "-u", "redis://guest:guest@127.0.0.1:6379", "ping"]
      interval: 10s
      timeout: 5s
      retries: 5
      start_period: 5s

  postgres_db:
    image: postgres:latest
    container_name: mi_postgres_db
    environment:
      POSTGRES_USER: usrtweeter
      POSTGRES_PASSWORD: 123456
      POSTGRES_DB: tweeter
    networks:
      - uala_net
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data
    restart: unless-stopped

volumes:
  rabbitmq_data:
    driver: local
  redis_data:
    driver: local
  postgres_data:
    driver: local

networks:
  uala_net:
    driver: bridge
```

## Gestión de Migraciones de Base de Datos PostgreSQL con Entity Framework Core
En el caso de que el modelo de dato sufra algun cambio debemos ejecutar la migracion de la misma para que la estructura de datos sea consistente entre el modelo y la base de datos.

### Introducción
Cuando se producen modificaciones en el modelo de datos de la aplicación, es crucial actualizar el esquema de la base de datos PostgreSQL para reflejar dichos cambios y mantener la consistencia. Este proceso se gestiona mediante "migraciones" de Entity Framework Core, que permiten versionar y aplicar evolutivamente los cambios en la estructura de la base de datos.

#### Proceso de Migración

##### Creación de una Nueva Migración
Si se han realizado cambios en el modelo de datos (ej. adición/eliminación de entidades, modificación de propiedades, nuevas relaciones), es necesario generar un nuevo archivo de migración que contenga el código para aplicar estos cambios al esquema de la base de datos.

- **Acción**: Abra una terminal o línea de comandos (CMD, PowerShell, Bash, etc.).
- **Ubicación**: Navegue hasta el directorio del proyecto que contiene su clase ``DbContext`` (en este caso, el proyecto ``TweeterDB`` que define ``TweetContext``).
- **Comando**:
```
dotnet ef migrations add NombreDescriptivoDeLaMigracion --context TweetContext --project TweeterDB
```

##### Aplicación de las Migraciones a la Base de Datos
Una vez creada la migración, los cambios definidos en ella deben aplicarse a la base de datos PostgreSQL para que el esquema se actualice.

- **Acción**: Desde la misma terminal y ubicación (o asegurándose de que el contexto del comando sea el adecuado).
- **Comando**:
```
dotnet ef database update --context TweetContext --startup-project TweeterDB/TweeterDB.csproj
```

## Utilizacion de servicios

### SaveTweet
**Método HTTP:** ``POST``

**Endpoint URL:** ``https://localhost:7098/gateway/tweet/save``

**Cabeceras (Headers)**
| Clave | Valor | Descripción |
| ----- | ----- | ----------- |
| User | 1 | Id del usuario |

**Cuerpo de la Petición (Body)**
```
{
  "message": "Mensaje corto"
}
```

**cURL**
```
curl --location --request POST 'https://localhost:7098/gateway/tweet/save' \
--header 'User: {id_del_usuario}' \
--header 'Content-Type: application/json' \
--data-raw '{
    "message": "Hoy es el ultimo dia del entregable"
}'
```

---

### SaveFollow
**Método HTTP:** ``POST``

**Endpoint URL:** ``https://localhost:7098/gateway/follow/save``

**Cabeceras (Headers)**
| Clave | Valor | Descripción |
| ----- | ----- | ----------- |
| User | 1 | Id del usuario |

**Cuerpo de la Petición (Body)**
```
{
  "followTo": 2
}
```

**cURL**
```
curl --location --request POST 'https://localhost:7098/gateway/follow/save' \
--header 'User: {id_del_usuario}' \
--header 'Content-Type: application/json' \
--data-raw '{
    "followTo": 2
}'
```

---

### GetTimeline
**Método HTTP:** ``GET``

**Endpoint URL:** ``https://localhost:7098/gateway/timeline``

**Cabeceras (Headers)**
| Clave | Valor | Descripción |
| ----- | ----- | ----------- |
| User | 1 | Id del usuario |

**Cuerpo de la Petición (Body)**
```
{
    "page": 1,
    "pagSize": 25
}
```

**cURL**
```
curl --location --request GET 'https://localhost:7098/gateway/timeline' \
--header 'User: {id_del_usuario}' \
--header 'Content-Type: application/json' \
--data-raw '{
    "page": 1,
    "pagSize": 25
}'
```