FROM node:17-alpine as builder
ENV NODE_ENV=production
WORKDIR /app

COPY ["MiniTwit.Web/package.json", "MiniTwit.Web/package-lock.json*", "./"]
RUN npm install --omit=dev
RUN npm install --location=global serve

# add app
COPY 'MiniTwit.Web/.' .

RUN npm run build

RUN echo "if [ "\$NODE_ENV" = 'development' ]; then npm run start; else serve -s build; fi;" > run.sh

RUN chmod 777 ./run.sh;
CMD ./run.sh


