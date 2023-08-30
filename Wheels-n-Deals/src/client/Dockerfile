FROM node:18 AS build
WORKDIR /client

EXPOSE 4200

RUN npm install -g @angular/cli

COPY package*.json ./
RUN npm install

COPY . .

CMD ng serve --host 0.0.0.0 --port 4200 --disable-host-check --open
