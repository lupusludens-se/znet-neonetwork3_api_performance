FROM node:18.19 AS ui-build

ARG env_name
ARG ing_path

WORKDIR /usr/src/app
COPY . .

RUN npm install @angular/cli && npm install
#Active the syncfusion license - this is dependent on a file in the application root to exist syncfusion-license.txt

RUN npx syncfusion-license activate
RUN node_modules/.bin/ng build --configuration="${env_name}" --base-href="${ing_path}/" --deploy-url="${ing_path}/"

FROM nginxinc/nginx-unprivileged:1.25-alpine-slim

ARG env_name
ARG ing_path

COPY --from=ui-build /usr/src/app/dist/zeigonetwork "/usr/share/nginx/html${ing_path}/"
COPY --from=ui-build /usr/src/app/dist/zeigonetwork/index.html /usr/share/nginx/html/index.html