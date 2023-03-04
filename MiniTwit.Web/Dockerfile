FROM node:17-alpine as builder
WORKDIR /app
EXPOSE 3000

# COPY ./package.json .
# COPY ./package-lock.json .
COPY ["MiniTwit.Web/package.json", "MiniTwit.Web/package-lock.json*", "./"]
RUN npm install
RUN npm install "axios"

# add app
COPY 'MiniTwit.Web/.' .

# start app
CMD ["npm", "start"]