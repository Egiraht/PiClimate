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
        class MeasurementsResult {
            constructor() {
                this.fromTime = null;
                this.toTime = null;
                this.resolution = null;
                this.measurements = null;
            }
            static fetchFromJson(url, method, filter) {
                return __awaiter(this, void 0, void 0, function* () {
                    try {
                        let response = yield fetch(url, {
                            method: method,
                            mode: "cors",
                            headers: {
                                "Accept": "application/json",
                                "Content-Type": "application/json"
                            },
                            body: JSON.stringify(filter)
                        });
                        return yield response.json();
                    }
                    catch (_a) {
                        return null;
                    }
                });
            }
        }
        Monitor.MeasurementsResult = MeasurementsResult;
    })(Monitor = PiClimate.Monitor || (PiClimate.Monitor = {}));
})(PiClimate || (PiClimate = {}));
var PiClimate;
(function (PiClimate) {
    var Monitor;
    (function (Monitor) {
        class MeasurementFilter {
            constructor() {
                this.fromTime = null;
                this.toTime = null;
                this.resolution = null;
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
                this.chartId = "";
                this.chartLabel = "";
                this.measurementParameter = "";
                this.lineColor = "";
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
                    this.chart = new Chart(this.chartParameters.chartId, {
                        type: "scatter",
                        data: {
                            datasets: [
                                {
                                    label: this.chartParameters.chartLabel,
                                    data: response.measurements.map(measurement => {
                                        return {
                                            x: measurement.d,
                                            y: measurement[this.chartParameters.measurementParameter]
                                        };
                                    }),
                                    radius: 0,
                                    showLine: true,
                                    backgroundColor: this.chartParameters.lineColor,
                                    borderColor: this.chartParameters.lineColor,
                                    borderWidth: 2,
                                    lineTension: 0,
                                    fill: false
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
                                        }
                                    }
                                ]
                            },
                            tooltips: {
                                mode: "index",
                                intersect: false
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
