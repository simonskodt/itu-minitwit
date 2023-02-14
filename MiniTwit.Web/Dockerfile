FROM node:17-alpine as builder
WORKDIR /app
EXPOSE 3000

# COPY ./package.json .
# COPY ./package-lock.json .
COPY ["MiniTwit/MiniTwit.Web/package.json", "MiniTwit/MiniTwit.Web/package-lock.json*", "./"]
RUN npm install

# add app
COPY 'MiniTwit/MiniTwit.Web/.' .

# start app
CMD ["npm", "start"]