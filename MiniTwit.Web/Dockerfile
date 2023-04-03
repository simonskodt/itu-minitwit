FROM node:19.7-alpine as builder
WORKDIR /app

COPY ["MiniTwit.Web/package.json", "MiniTwit.Web/package-lock.json*", "./"]
RUN npm install --omit=dev
RUN npm install --location=global serve

# add app
COPY 'MiniTwit.Web/.' .
RUN npm run build

# start app
CMD [ "serve", "-s", "build"]
