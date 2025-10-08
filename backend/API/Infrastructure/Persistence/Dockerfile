FROM mysql:8.0.35

ENV MYSQL_ROOT_PASSWORD=root
ENV MYSQL_DATABASE=trainin_database
ENV MYSQL_USER=admin
ENV MYSQL_PASSWORD=passwd

COPY ./dump.sql /docker-entrypoint-initdb.d/

EXPOSE 3306
