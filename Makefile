SLN ?= PolyhydraGames.Core.Models.sln
CONFIGURATION ?= Release
TEST_TIMEOUT ?= 20m
HANG_TIMEOUT ?= 5m

.PHONY: restore build test coverage mutate format lint ci clean

restore:
	dotnet restore $(SLN)
	dotnet tool restore

build:
	dotnet build $(SLN) -c $(CONFIGURATION) --no-restore

test:
	timeout -k 30s $(TEST_TIMEOUT) dotnet test $(SLN) -c $(CONFIGURATION) --no-build --blame-hang-timeout $(HANG_TIMEOUT) --blame-hang-dump-type mini --results-directory TestResults --logger trx

coverage:
	timeout -k 30s $(TEST_TIMEOUT) dotnet test $(SLN) -c $(CONFIGURATION) --no-build --blame-hang-timeout $(HANG_TIMEOUT) --blame-hang-dump-type mini --results-directory TestResults --logger trx --collect:"XPlat Code Coverage"
	python3 scripts/summarize-coverage.py TestResults

mutate:
	dotnet stryker --config-file stryker-config.json

format:
	dotnet format $(SLN)

lint:
	dotnet format $(SLN) --verify-no-changes
	CI=true dotnet build $(SLN) -c Release --no-restore

ci: restore lint test coverage

clean:
	rm -rf Models/bin Models/obj Tests/bin Tests/obj TestResults StrykerOutput
