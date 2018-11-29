(function ($, window, undefined) {
    /// <param name="$" type="jQuery" />
    "use strict";

    if (typeof ($.signalR) !== "function") {
        throw new Error("SignalR: SignalR is not loaded. Please ensure jquery.signalR-x.js is referenced before ~/signalr/js.");
    }

    var signalR = $.signalR;

    function makeProxyCallback(hub, callback) {
        return function () {
            // Call the client hub method
            callback.apply(hub, $.makeArray(arguments));
        };
    }

    function registerHubProxies(instance, shouldSubscribe) {
        var key, hub, memberKey, memberValue, subscriptionMethod;

        for (key in instance) {
            if (instance.hasOwnProperty(key)) {
                hub = instance[key];

                if (!(hub.hubName)) {
                    // Not a client hub
                    continue;
                }

                if (shouldSubscribe) {
                    // We want to subscribe to the hub events
                    subscriptionMethod = hub.on;
                } else {
                    // We want to unsubscribe from the hub events
                    subscriptionMethod = hub.off;
                }

                // Loop through all members on the hub and find client hub functions to subscribe/unsubscribe
                for (memberKey in hub.client) {
                    if (hub.client.hasOwnProperty(memberKey)) {
                        memberValue = hub.client[memberKey];

                        if (!$.isFunction(memberValue)) {
                            // Not a client hub function
                            continue;
                        }

                        // Use the actual user-provided callback as the "identity" value for the registration.
                        subscriptionMethod.call(hub, memberKey, makeProxyCallback(hub, memberValue), memberValue);
                    }
                }
            }
        }
    }

    $.hubConnection.prototype.createHubProxies = function () {
        var proxies = {};
        this.starting(function () {
            // Register the hub proxies as subscribed
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, true);

            this._registerSubscribedHubs();
        }).disconnected(function () {
            // Unsubscribe all hub proxies when we "disconnect".  This is to ensure that we do not re-add functional call backs.
            // (instance, shouldSubscribe)
            registerHubProxies(proxies, false);
        });

        proxies['consoleHub'] = this.createHubProxy('consoleHub');
        proxies['consoleHub'].client = {};
        proxies['consoleHub'].server = {
            error: function (msg) {
                return proxies['consoleHub'].invoke.apply(proxies['consoleHub'], $.merge(["Error"], $.makeArray(arguments)));
            },

            info: function (msg) {
                return proxies['consoleHub'].invoke.apply(proxies['consoleHub'], $.merge(["Info"], $.makeArray(arguments)));
            },

            log: function (msg) {
                return proxies['consoleHub'].invoke.apply(proxies['consoleHub'], $.merge(["Log"], $.makeArray(arguments)));
            },

            regist: function (name) {
                return proxies['consoleHub'].invoke.apply(proxies['consoleHub'], $.merge(["Regist"], $.makeArray(arguments)));
            },

            warn: function (msg) {
                return proxies['consoleHub'].invoke.apply(proxies['consoleHub'], $.merge(["Warn"], $.makeArray(arguments)));
            },

            debug: function (msg) {
                return proxies['consoleHub'].invoke.apply(proxies['consoleHub'], $.merge(["Debug"], $.makeArray(arguments)));
            }
        };

        return proxies;
    };
    var scriptSrc = document.getElementsByTagName('script')[document.getElementsByTagName('script').length - 1].src;
    var signalrUrl = scriptSrc.toLowerCase().replace("scripts/client.js", "") + "signalr";
    console.log("当前远程日志服务连接为：" + signalrUrl);
    signalR.hub = $.hubConnection(signalrUrl, { useDefaultPath: false });
    $.extend(signalR, signalR.hub.createHubProxies());

}(window.jQuery, window));

(function () {
    var consoleHub = $.connection.consoleHub;
    $.connection.hub.start().done(function () {
        consoleHub.server.regist(Math.random());
        console.log("远程日志服务连接成功，正在重写本地的console方法...");
        var oldLog = console.log;
        var oldInfo = console.info;
        var oldWarn = console.warn;
        var oldError = console.error;
        var oldDebug = console.debug;
        console.log = function (message) {
            consoleHub.server.log(message);
            oldLog.apply(console, arguments);
        };
        console.info = function (message) {
            consoleHub.server.info(message);
            oldInfo.apply(console, arguments);
        };
        console.warn = function (message) {
            consoleHub.server.warn(message);
            oldWarn.apply(console, arguments);
        };
        console.error = function (message) {
            consoleHub.server.error(message);
            oldError.apply(console, arguments);
        };
        console.debug = function (message) {
            consoleHub.server.debug(message);
            oldDebug.apply(console, arguments);
        };
    });
})();