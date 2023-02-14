FROM node:17-alpine as builder
WORKDIR /app
EXPOSE 3000

COPY package.json .
COPY package-lock.json .
RUN npm install

# add app
COPY . ./

# start app
CMD ["npm", "start"]