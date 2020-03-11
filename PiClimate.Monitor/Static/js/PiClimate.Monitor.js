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
                this.timePeriod = 24 * 60 * 60;
                this.fromTime = new Date(Date.now() - 24 * 60 * 60 * 1000);
                this.toTime = new Date();
                this.resolution = 1440;
            }
        }
        Monitor.MeasurementFilter = MeasurementFilter;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class ChartParameters {
            constructor() {
                this.chartId = "measurements-chart";
                this.pressureChartLabel = "Pressure";
                this.temperatureChartLabel = "Temperature";
                this.humidityChartLabel = "Humidity";
                this.pressureLineColor = "blue";
                this.temperatureLineColor = "green";
                this.humidityLineColor = "red";
                this.requestUri = "";
                this.requestMethod = "POST";
                this.filter = new Monitor.MeasurementFilter();
            }
        }
        Monitor.ChartParameters = ChartParameters;
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
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class MeasurementsCollection {
            constructor() {
                this.minTime = new Date();
                this.maxTime = new Date();
                this.minPressure = 0;
                this.maxPressure = 0;
                this.minPressureTime = new Date();
                this.maxPressureTime = new Date();
                this.minTemperature = 0;
                this.maxTemperature = 0;
                this.minTemperatureTime = new Date();
                this.maxTemperatureTime = new Date();
                this.minHumidity = 0;
                this.maxHumidity = 0;
                this.minHumidityTime = new Date();
                this.maxHumidityTime = new Date();
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
        class ChartControl {
            constructor(chartParameters) {
                this.chart = null;
                this.chartParameters = chartParameters;
            }
            fetchFromJson() {
                return __awaiter(this, void 0, void 0, function* () {
                    try {
                        let response = yield fetch(this.chartParameters.requestUri, {
                            method: this.chartParameters.requestMethod,
                            mode: "cors",
                            headers: {
                                "Accept": "application/json",
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(this.chartParameters.filter)
                        });
                        return yield response.json();
                    }
                    catch (_a) {
                        return null;
                    }
                });
            }
            updateChart() {
                return __awaiter(this, void 0, void 0, function* () {
                    let response = yield this.fetchFromJson();
                    if (!response || !response.measurements)
                        return false;
                    let defaults = Chart.defaults.global.elements;
                    defaults.point.radius = 0.5;
                    defaults.point.hitRadius = 0;
                    defaults.point.hoverRadius = 0;
                    defaults.line.borderWidth = 2;
                    defaults.line.tension = 0;
                    defaults.line.fill = false;
                    this.chart = new Chart(this.chartParameters.chartId, {
                        type: "scatter",
                        data: {
                            datasets: [
                                {
                                    label: this.chartParameters.pressureChartLabel,
                                    yAxisID: this.chartParameters.pressureChartLabel,
                                    backgroundColor: this.chartParameters.pressureLineColor,
                                    borderColor: this.chartParameters.pressureLineColor,
                                    showLine: true,
                                    data: response.measurements.map(measurement => {
                                        return {
                                            x: measurement.d,
                                            y: measurement.p
                                        };
                                    })
                                },
                                {
                                    label: this.chartParameters.temperatureChartLabel,
                                    yAxisID: this.chartParameters.temperatureChartLabel,
                                    backgroundColor: this.chartParameters.temperatureLineColor,
                                    borderColor: this.chartParameters.temperatureLineColor,
                                    showLine: true,
                                    data: response.measurements.map(measurement => {
                                        return {
                                            x: measurement.d,
                                            y: measurement.t
                                        };
                                    })
                                },
                                {
                                    label: this.chartParameters.humidityChartLabel,
                                    yAxisID: this.chartParameters.humidityChartLabel,
                                    backgroundColor: this.chartParameters.humidityLineColor,
                                    borderColor: this.chartParameters.humidityLineColor,
                                    showLine: true,
                                    data: response.measurements.map(measurement => {
                                        return {
                                            x: measurement.d,
                                            y: measurement.h
                                        };
                                    })
                                }
                            ],
                        },
                        options: {
                            scales: {
                                xAxes: [
                                    {
                                        type: "time",
                                        time: {
                                            isoWeekday: true,
                                            minUnit: "second",
                                            displayFormats: {
                                                second: "HH:mm:ss",
                                                minute: "HH:mm",
                                                hour: "HH:00"
                                            }
                                        },
                                        ticks: {
                                            min: response.minTime.valueOf() < this.chartParameters.filter.fromTime.valueOf()
                                                ? response.minTime
                                                : this.chartParameters.filter.fromTime,
                                            max: response.maxTime.valueOf() > this.chartParameters.filter.toTime.valueOf()
                                                ? response.maxTime
                                                : this.chartParameters.filter.toTime
                                        }
                                    }
                                ],
                                yAxes: [
                                    {
                                        id: this.chartParameters.pressureChartLabel,
                                        type: "linear",
                                        position: "left",
                                        scaleLabel: {
                                            display: true,
                                            labelString: this.chartParameters.pressureChartLabel,
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
                                        id: this.chartParameters.temperatureChartLabel,
                                        type: "linear",
                                        position: "left",
                                        scaleLabel: {
                                            display: true,
                                            labelString: this.chartParameters.temperatureChartLabel,
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
                                        id: this.chartParameters.humidityChartLabel,
                                        type: "linear",
                                        position: "left",
                                        scaleLabel: {
                                            display: true,
                                            labelString: this.chartParameters.humidityChartLabel,
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
                                intersect: false,
                                position: "nearest"
                            },
                            animation: {
                                duration: 500
                            }
                        }
                    });
                    return true;
                });
            }
        }
        Monitor.ChartControl = ChartControl;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
