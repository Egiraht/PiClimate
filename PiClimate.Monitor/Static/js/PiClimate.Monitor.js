"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class MeasurementFilter {
            constructor() {
                this._resolution = MeasurementFilter.defaultResolution;
                this.fromTime = new Date(Date.now() - MeasurementFilter.defaultTimePeriod).toISOString();
                this.toTime = new Date().toISOString();
            }
            get timePeriod() {
                return Math.round(Math.abs(new Date(this.toTime).valueOf() - new Date(this.fromTime).valueOf()) / 1000);
            }
            set timePeriod(value) {
                this.fromTime = new Date(new Date(this.toTime).valueOf() - value * 1000).toISOString();
            }
            get resolution() {
                return this._resolution;
            }
            set resolution(value) {
                this._resolution = Math.max(value, MeasurementFilter.minimalResolution);
            }
        }
        MeasurementFilter.minimalResolution = 1;
        MeasurementFilter.defaultResolution = 1440;
        MeasurementFilter.defaultTimePeriod = 24 * 60 * 60 * 1000;
        Monitor.MeasurementFilter = MeasurementFilter;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class ChartParameters {
            constructor(chartId) {
                this.pressureChartLabel = "Pressure";
                this.temperatureChartLabel = "Temperature";
                this.humidityChartLabel = "Humidity";
                this.pressureUnits = "mmHg";
                this.temperatureUnits = "°C";
                this.humidityUnits = "%";
                this.pressureLineColor = "blue";
                this.temperatureLineColor = "green";
                this.humidityLineColor = "red";
                this.requestUri = "";
                this.requestMethod = "POST";
                this.filter = new Monitor.MeasurementFilter();
                this.chartId = chartId;
            }
        }
        Monitor.ChartParameters = ChartParameters;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class MeasurementsCollection {
            constructor() {
                this.minTimestamp = new Date().toISOString();
                this.maxTimestamp = new Date().toISOString();
                this.minPressure = 0;
                this.maxPressure = 0;
                this.minPressureTimestamp = new Date().toISOString();
                this.maxPressureTimestamp = new Date().toISOString();
                this.minTemperature = 0;
                this.maxTemperature = 0;
                this.minTemperatureTimestamp = new Date().toISOString();
                this.maxTemperatureTimestamp = new Date().toISOString();
                this.minHumidity = 0;
                this.maxHumidity = 0;
                this.minHumidityTimestamp = new Date().toISOString();
                this.maxHumidityTimestamp = new Date().toISOString();
                this.count = 0;
                this.measurements = [];
            }
        }
        Monitor.MeasurementsCollection = MeasurementsCollection;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class ChartComponent {
            constructor(chartParameters) {
                this._isUpdating = false;
                this._isEmpty = true;
                this._isUpdatingFailed = false;
                this.chartParameters = chartParameters;
                let defaults = Chart.defaults.global.elements;
                defaults.point.radius = 0.5;
                defaults.point.hitRadius = 5;
                defaults.point.hoverRadius = 5;
                defaults.line.borderWidth = 2;
                defaults.line.tension = 0;
                defaults.line.fill = false;
                this.chart = new Chart(this.chartParameters.chartId, {
                    type: "scatter",
                    data: {
                        datasets: [
                            {
                                xAxisID: ChartComponent.dateAxisId,
                                yAxisID: ChartComponent.pressureAxisId,
                                label: this.chartParameters.pressureChartLabel,
                                backgroundColor: this.chartParameters.pressureLineColor,
                                borderColor: this.chartParameters.pressureLineColor,
                                showLine: true,
                                data: []
                            },
                            {
                                xAxisID: ChartComponent.dateAxisId,
                                yAxisID: ChartComponent.temperatureAxisId,
                                label: this.chartParameters.temperatureChartLabel,
                                backgroundColor: this.chartParameters.temperatureLineColor,
                                borderColor: this.chartParameters.temperatureLineColor,
                                showLine: true,
                                data: []
                            },
                            {
                                xAxisID: ChartComponent.dateAxisId,
                                yAxisID: ChartComponent.humidityAxisId,
                                label: this.chartParameters.humidityChartLabel,
                                backgroundColor: this.chartParameters.humidityLineColor,
                                borderColor: this.chartParameters.humidityLineColor,
                                showLine: true,
                                data: []
                            }
                        ]
                    },
                    options: {
                        scales: {
                            xAxes: [
                                {
                                    id: ChartComponent.dateAxisId,
                                    type: "time",
                                    display: true,
                                    time: {
                                        isoWeekday: true,
                                        minUnit: "second",
                                        displayFormats: {
                                            second: "HH:mm:ss",
                                            minute: "HH:mm",
                                            hour: "HH:00"
                                        }
                                    },
                                    ticks: {}
                                }
                            ],
                            yAxes: [
                                {
                                    id: ChartComponent.pressureAxisId,
                                    type: "linear",
                                    display: "auto",
                                    position: "left",
                                    scaleLabel: {
                                        display: true,
                                        labelString: `${this.chartParameters.pressureChartLabel}, ${this.chartParameters.pressureUnits}`,
                                        fontColor: this.chartParameters.pressureLineColor
                                    },
                                    gridLines: {
                                        color: this.chartParameters.pressureLineColor,
                                        lineWidth: 0.5
                                    },
                                    ticks: {
                                        fontColor: this.chartParameters.pressureLineColor
                                    }
                                },
                                {
                                    id: ChartComponent.temperatureAxisId,
                                    type: "linear",
                                    display: "auto",
                                    position: "right",
                                    scaleLabel: {
                                        display: true,
                                        labelString: `${this.chartParameters.temperatureChartLabel}, ${this.chartParameters.temperatureUnits}`,
                                        fontColor: this.chartParameters.temperatureLineColor
                                    },
                                    gridLines: {
                                        color: this.chartParameters.temperatureLineColor,
                                        lineWidth: 0.5
                                    },
                                    ticks: {
                                        fontColor: this.chartParameters.temperatureLineColor
                                    }
                                },
                                {
                                    id: ChartComponent.humidityAxisId,
                                    type: "linear",
                                    display: "auto",
                                    position: "right",
                                    scaleLabel: {
                                        display: true,
                                        labelString: `${this.chartParameters.humidityChartLabel}, ${this.chartParameters.humidityUnits}`,
                                        fontColor: this.chartParameters.humidityLineColor
                                    },
                                    gridLines: {
                                        color: this.chartParameters.humidityLineColor,
                                        lineWidth: 0.5
                                    },
                                    ticks: {
                                        fontColor: this.chartParameters.humidityLineColor
                                    }
                                }
                            ]
                        },
                        tooltips: {
                            mode: "index",
                            intersect: true,
                            position: "nearest",
                            callbacks: {
                                title: (tooltipItems) => new Date(tooltipItems[0].xLabel).toLocaleString(),
                                label: (tooltipItem) => {
                                    let units = [
                                        this.chartParameters.pressureUnits,
                                        this.chartParameters.temperatureUnits,
                                        this.chartParameters.humidityUnits
                                    ];
                                    return `${tooltipItem.yLabel} ${units[tooltipItem.datasetIndex]}`;
                                }
                            }
                        },
                        animation: {
                            duration: 500
                        }
                    }
                });
            }
            get isEmpty() {
                return this._isEmpty;
            }
            set isEmpty(value) {
                this._isEmpty = value;
                let chartWrapperElement = $(`#${this.chartParameters.chartId}-wrapper`);
                if (this._isEmpty)
                    chartWrapperElement.addClass(ChartComponent.emptyClassName);
                else
                    chartWrapperElement.removeClass(ChartComponent.emptyClassName);
            }
            get isUpdating() {
                return this._isUpdating;
            }
            set isUpdating(value) {
                this._isUpdating = value;
                let chartWrapperElement = $(`#${this.chartParameters.chartId}-wrapper`);
                if (this._isUpdating)
                    chartWrapperElement.addClass(ChartComponent.updatingClassName);
                else
                    chartWrapperElement.removeClass(ChartComponent.updatingClassName);
            }
            get isUpdatingFailed() {
                return this._isUpdatingFailed;
            }
            set isUpdatingFailed(value) {
                this._isUpdatingFailed = value;
                let chartWrapperElement = $(`#${this.chartParameters.chartId}-wrapper`);
                if (this._isUpdatingFailed)
                    chartWrapperElement.addClass(ChartComponent.updatingFailedClassName);
                else
                    chartWrapperElement.removeClass(ChartComponent.updatingFailedClassName);
            }
            fetchFromJson() {
                var _a;
                return __awaiter(this, void 0, void 0, function* () {
                    try {
                        this.isEmpty = false;
                        this.isUpdatingFailed = false;
                        this.isUpdating = true;
                        let response = yield fetch(this.chartParameters.requestUri, {
                            method: this.chartParameters.requestMethod,
                            mode: "cors",
                            headers: {
                                "Accept": "application/json",
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify({
                                resolution: this.chartParameters.filter.resolution,
                                fromTime: new Date(this.chartParameters.filter.fromTime)
                                    .toISOString()
                                    .replace(/Z$/ig, "+00:00"),
                                toTime: new Date(this.chartParameters.filter.toTime)
                                    .toISOString()
                                    .replace(/Z$/ig, "+00:00")
                            })
                        });
                        if (!response.ok) {
                            throw response;
                        }
                        let data = yield response.json();
                        this.isEmpty = !((_a = data.measurements) === null || _a === void 0 ? void 0 : _a.length);
                        return Object.assign(new Monitor.MeasurementsCollection(), data);
                    }
                    catch (_b) {
                        this.isUpdatingFailed = true;
                        return null;
                    }
                    finally {
                        this.isUpdating = false;
                    }
                });
            }
            updateChart() {
                return __awaiter(this, void 0, void 0, function* () {
                    let response = yield this.fetchFromJson();
                    if (!response || !response.measurements)
                        return false;
                    this.chart.data.datasets[0].data = response.measurements.map(measurement => {
                        return {
                            x: measurement.d,
                            y: measurement.p
                        };
                    });
                    this.chart.data.datasets[1].data = response.measurements.map(measurement => {
                        return {
                            x: measurement.d,
                            y: measurement.t
                        };
                    });
                    this.chart.data.datasets[2].data = response.measurements.map(measurement => {
                        return {
                            x: measurement.d,
                            y: measurement.h
                        };
                    });
                    this.chart.options.scales.xAxes[0].ticks = {
                        min: new Date(response.minTimestamp).valueOf() < new Date(this.chartParameters.filter.fromTime).valueOf()
                            ? new Date(response.minTimestamp)
                            : new Date(this.chartParameters.filter.fromTime),
                        max: new Date(response.maxTimestamp).valueOf() > new Date(this.chartParameters.filter.toTime).valueOf()
                            ? new Date(response.maxTimestamp)
                            : new Date(this.chartParameters.filter.toTime)
                    };
                    this.chart.update();
                    this.updateChartSummary(response);
                    return true;
                });
            }
            updateChartSummary(response) {
                let $ = jQuery;
                let periodStartElement = $(`#${this.chartParameters.chartId}-summary .period-start`);
                let periodEndElement = $(`#${this.chartParameters.chartId}-summary .period-end`);
                let minPressureElement = $(`#${this.chartParameters.chartId}-summary .min-pressure`);
                let maxPressureElement = $(`#${this.chartParameters.chartId}-summary .max-pressure`);
                let minPressureTimestampElement = $(`#${this.chartParameters.chartId}-summary .min-pressure-timestamp`);
                let maxPressureTimestampElement = $(`#${this.chartParameters.chartId}-summary .max-pressure-timestamp`);
                let minTemperatureElement = $(`#${this.chartParameters.chartId}-summary .min-temperature`);
                let maxTemperatureElement = $(`#${this.chartParameters.chartId}-summary .max-temperature`);
                let minTemperatureTimestampElement = $(`#${this.chartParameters.chartId}-summary .min-temperature-timestamp`);
                let maxTemperatureTimestampElement = $(`#${this.chartParameters.chartId}-summary .max-temperature-timestamp`);
                let minHumidityElement = $(`#${this.chartParameters.chartId}-summary .min-humidity`);
                let maxHumidityElement = $(`#${this.chartParameters.chartId}-summary .max-humidity`);
                let minHumidityTimestampElement = $(`#${this.chartParameters.chartId}-summary .min-humidity-timestamp`);
                let maxHumidityTimestampElement = $(`#${this.chartParameters.chartId}-summary .max-humidity-timestamp`);
                let entriesCountElement = $(`#${this.chartParameters.chartId}-summary .entries-count`);
                periodStartElement.text(this.chart.options.scales.xAxes[0].ticks.min.toLocaleString());
                periodEndElement.text(this.chart.options.scales.xAxes[0].ticks.max.toLocaleString());
                minPressureElement.text(response.minPressure);
                maxPressureElement.text(response.maxPressure);
                minPressureTimestampElement.text(new Date(response.minPressureTimestamp).toLocaleString());
                maxPressureTimestampElement.text(new Date(response.maxPressureTimestamp).toLocaleString());
                minTemperatureElement.text(response.minTemperature);
                maxTemperatureElement.text(response.maxTemperature);
                minTemperatureTimestampElement.text(new Date(response.minTemperatureTimestamp).toLocaleString());
                maxTemperatureTimestampElement.text(new Date(response.maxTemperatureTimestamp).toLocaleString());
                minHumidityElement.text(response.minHumidity);
                maxHumidityElement.text(response.maxHumidity);
                minHumidityTimestampElement.text(new Date(response.minHumidityTimestamp).toLocaleString());
                maxHumidityTimestampElement.text(new Date(response.maxHumidityTimestamp).toLocaleString());
                entriesCountElement.text(response.measurements.length);
            }
        }
        ChartComponent.emptyClassName = "empty";
        ChartComponent.updatingClassName = "updating";
        ChartComponent.updatingFailedClassName = "failed";
        ChartComponent.dateAxisId = "date";
        ChartComponent.pressureAxisId = "pressure";
        ChartComponent.temperatureAxisId = "temperature";
        ChartComponent.humidityAxisId = "humidity";
        Monitor.ChartComponent = ChartComponent;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class Measurement {
            constructor() {
                this.d = null;
                this.p = null;
                this.t = null;
                this.h = null;
            }
        }
        Monitor.Measurement = Measurement;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
