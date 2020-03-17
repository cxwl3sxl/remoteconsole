let scriptSrc = document.getElementsByTagName('script')[document.getElementsByTagName('script').length - 1].src;
let url = new URL(scriptSrc);
let channel = url.searchParams.get('channel');
//let api = url.protocol + "//" + url.host + "/api/log/send";
let api = "http://122.51.155.15:8688/api/log/send";
//console.debug("Remote Log Channel：" + channel);
function send(data) {
    var xhr = new XMLHttpRequest()
    xhr.responseType = 'json';
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                //console.debug(xhr.response);
            } else {
                //console.warn("Submit Log Error", xhr.response);
            }
        }
    }
    xhr.open("POST", api, true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send(JSON.stringify(data).replace(/\n/, '<br/>'));
}
function getDatetime() {
    let now = new Date();
    return now.getFullYear() + '/' + (now.getMonth() + 1) + '/' + now.getDay() + ' ' + now.getHours() + ":" + now.getMinutes() + ":" + now.getSeconds();
}
function formatText(text) {
    if (typeof (text) === "string") return text;
    return JSON.stringify(text);
}
// define a new console
var myConsole = (function (oldCons) {
    return {
        trace: function (text) {
            oldCons.trace(text);
            text = formatText(text);
            send({
                category: channel,
                level: 'trace',
                msg: text,
                time: getDatetime()
            });
        },
        debug: function (text) {
            oldCons.debug(text);
            text = formatText(text);
            send({
                category: channel,
                level: 'debug',
                msg: text,
                time: getDatetime()
            });
        },
        log: function (text) {
            oldCons.log(text);
            text = formatText(text);
            send({
                category: channel,
                level: 'log',
                msg: text,
                time: getDatetime()
            });
        },
        info: function (text) {
            oldCons.info(text);
            text = formatText(text);
            send({
                category: channel,
                level: 'info',
                msg: text,
                time: getDatetime()
            });
        },
        warn: function (text) {
            oldCons.warn(text);
            text = formatText(text);
            send({
                category: channel,
                level: 'warn',
                msg: text,
                time: getDatetime()
            });
        },
        error: function (text) {
            oldCons.error(text);
            text = formatText(text);
            send({
                category: channel,
                level: 'error',
                msg: text,
                time: getDatetime()
            });
        }
    };
}(window.console));

//Then redefine the old console
window.console = myConsole;