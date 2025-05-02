SVC_IMAGE_NAME?=neonetwork3-service
SVC_EMAIL_ALERT_NAME?=neonetwork3-eml-alert
SVC_DATA_SYNC_NAME?=neonetwork3-data-sync
SPA_IMAGE_NAME?=nenetwork3-client
DB_IMAGE_NAME?=neonetwork3-migration
IMAGE_TAG?=local
ENVIRONMENT?=dev

svc-restore:
	@echo Restoring Service Project
	@dotnet restore server-app/SE.Neo.WebAPI/SE.Neo.WebAPI.csproj

svc-build: svc-restore
	@echo Building Service Project
	@dotnet build server-app/SE.Neo.WebAPI/SE.Neo.WebAPI.csproj

svc-clean:
	@echo Cleaning Service Project
	@rm -rf service/SE.Neo.WebAPI/bin \
					server-app/SE.Neo.WebAPI/obj \
					server-app/SE.Neo.WebAPI/SE.Neo.WebAPI.xml \
					.build/service

svc-publish: svc-build
	@echo Publishing Service Project
	@dotnet publish \
					-o .build/service \
	        server-app/SE.Neo.WebAPI/SE.Neo.WebAPI.csproj

svc-container: svc-publish
	@echo "Building Service Container $(SVC_IMAGE_NAME):$(IMAGE_TAG)"
	@docker build .build/service -f .cicd/api/Dockerfile -t $(SVC_IMAGE_NAME):$(IMAGE_TAG)

eml-restore:
	@echo Restoring Email Alert Project
	@dotnet restore server-app/SE.Neo.EmailAlertSender/SE.Neo.EmailAlertSender.csproj

eml-build: eml-restore
	@echo Building Email Alert Project
	@dotnet build server-app/SE.Neo.EmailAlertSender/SE.Neo.EmailAlertSender.csproj

eml-clean:
	@echo Cleaning Email Alert Project
	@rm -rf server-app/SE.Neo.EmailAlertSender/bin \
					server-app/SE.Neo.EmailAlertSender/obj \
					server-app/SE.Neo.EmailAlertSender/SE.Neo.WebAPI.xml \
					.build/eml

eml-publish: eml-build
	@echo Publishing Email Alert Project
	@dotnet publish \
					-o .build/eml \
	        server-app/SE.Neo.EmailAlertSender/SE.Neo.EmailAlertSender.csproj

eml-container: eml-publish
	@echo "Building Email Alert Container $(SVC_EMAIL_ALERT_NAME):$(IMAGE_TAG)"
	@docker build .build/eml -f .cicd/email-alert/Dockerfile -t $(SVC_EMAIL_ALERT_NAME):$(IMAGE_TAG)

sync-restore:
	@echo Restoring Data Sync Project
	@dotnet restore server-app/SE.Neo.DataSync/SE.Neo.DataSync.csproj

sync-build: sync-restore
	@echo Building Data Sync Project
	@dotnet build server-app/SE.Neo.DataSync/SE.Neo.DataSync.csproj

sync-clean:
	@echo Cleaning Data Sync Project
	@rm -rf server-app/SE.Neo.DataSync/bin \
					server-app/SE.Neo.DataSync/obj \
					server-app/SE.Neo.DataSync/SE.Neo.WebAPI.xml \
					.build/dsync

sync-publish: sync-build
	@echo Publishing Data Sync Project
	@dotnet publish \
					-o .build/dsync \
	        server-app/SE.Neo.DataSync/SE.Neo.DataSync.csproj

sync-container: sync-publish
	@echo "Building Data Sync Container $(SVC_DATA_SYNC_NAME):$(IMAGE_TAG)"
	@docker build .build/dsync -f .cicd/data-sync/Dockerfile -t $(SVC_DATA_SYNC_NAME):$(IMAGE_TAG)

spa-container:
	@echo "Building SPA Container $(SPA_IMAGE_NAME):$(IMAGE_TAG)"
	@docker build client-app -f .cicd/client/Dockerfile -t $(SPA_IMAGE_NAME):$(IMAGE_TAG) --build-arg env_name=$(ENVIRONMENT) --build-arg ing_path="/"

launch-spa: spa-container
	@echo "Launching SPA Container $(SPA_IMAGE_NAME):$(IMAGE_TAG)"
	@docker run --name $(SPA_IMAGE_NAME)_$(IMAGE_TAG) -p 127.0.0.1:8080:8080/tcp --rm --detach $(SPA_IMAGE_NAME):$(IMAGE_TAG)

stop-spa:
	@echo "Stopping SPA Container $(SPA_IMAGE_NAME):$(IMAGE_TAG)"
	@docker stop $(SPA_IMAGE_NAME)_$(IMAGE_TAG)

migration-container:
	@echo "Building Migration Container"
	@docker build server-app/ -f .cicd/migration/Dockerfile -t $(DB_IMAGE_NAME):$(IMAGE_TAG)
