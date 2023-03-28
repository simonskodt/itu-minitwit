FROM node:17-alpine as builder
ENV NODE_ENV=development
WORKDIR /app

COPY ["MiniTwit.Web/package.json", "MiniTwit.Web/package-lock.json*", "./"]
RUN npm install --omit=dev
RUN npm install -g serve

# add app
COPY 'MiniTwit.Web/.' .

RUN npm run build

RUN echo "if [ "\$NODE_ENV" = 'development' ]; then npm run start; else serve -s build; fi;" > run.sh

RUN chmod 777 ./run.sh;
CMD ./run.sh


# start app
