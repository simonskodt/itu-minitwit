FROM node:17-alpine as builder
WORKDIR /app
EXPOSE 3000

# COPY ./package.json .
# COPY ./package-lock.json .
COPY ["MiniTwit.Web/package.json", "MiniTwit.Web/package-lock.json*", "./"]
RUN npm install --omit=dev
RUN npm install -g serve

# add app
COPY 'MiniTwit.Web/.' .
RUN npm run build

# start app
CMD [ "serve", "-s", "build"]