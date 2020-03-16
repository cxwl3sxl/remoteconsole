import defaultSettings from '@/settings'

let Socket = null;
let PingInterval = null;
let connectCallBack;
let isLogined = false;

//#region webevent相关

//自定义事件构造函数
function EventTarget() {
    //事件处理程序数组集合
    this.handlers = {};
}
//自定义事件的原型对象
EventTarget.prototype = {
    //设置原型构造函数链
    constructor: EventTarget,
    //注册给定类型的事件处理程序，
    //type -> 自定义事件类型， handler -> 自定义事件回调函数
    addEvent: function (type, handler) {
        //判断事件处理数组是否有该类型事件
        if (typeof this.handlers[type] == 'undefined') {
            this.handlers[type] = [];
        }
        //将处理事件push到事件处理数组里面
        this.handlers[type].push(handler);
    },
    //触发一个事件
    //event -> 为一个js对象，属性中至少包含type属性，
    //因为类型是必须的，其次可以传一些处理函数需要的其他变量参数。（这也是为什么要传js对象的原因）
    fireEvent: function (event) {
        //模拟真实事件的event
        // if (!event.target) {
        //     event.target = this;
        // }
        //判断是否存在该事件类型
        if (this.handlers[event.type] instanceof Array) {
            var handlers = this.handlers[event.type];
            //在同一个事件类型下的可能存在多种处理事件，找出本次需要处理的事件
            for (var i = 0; i < handlers.length; i++) {
                //执行触发
                handlers[i](event.data);
            }
        }
    },
    //注销事件
    //type -> 自定义事件类型， handler -> 自定义事件回调函数
    removeEvent: function (type, handler) {
        //判断是否存在该事件类型
        if (this.handlers[type] instanceof Array) {
            var handlers = this.handlers[type];
            //在同一个事件类型下的可能存在多种处理事件
            for (var i = 0; i < handlers.length; i++) {
                //找出本次需要处理的事件下标
                if (handlers[i] == handler) {
                    break;
                }
            }
            //从事件处理数组里面删除
            handlers.splice(i, 1);
        }
    },

    /**
     * 获取指定事件订阅的数量
     *
     * @param {*} type
     * @returns
     */
    getHandlerCounts: function (type) {
        if (this.handlers[type] instanceof Array) {
            return this.handlers[type].length;
        }
        return 0;
    },

    /**
     * 获取所有已经被订阅的事件
     *
     * @returns
     */
    allsubscribeEvents: function () {
        var list = [];
        for (var e in this.handlers) {
            list.push({
                type: e,
                count: this.handlers[e].length
            });
        }
        return list;
    }
};

/*
* 系统预设的Socket事件
*/
export const SocketEvent = {
    Connected: "Socket_Connected",
    DisConnected: "Socket_DisConnected"
}

/**
 * webevent定义
 * 系统预设 Socket_Connected事件 和 Socket_DisConnected事件
 *
 * @class WebEvent
 */
class WebEvent {

    /**
     *Creates an instance of WebEvent.
     * @param {*} subscribeChanged 两个参数，第一个为事件类型，第二为 当前订阅数量
     * @memberof WebEvent
     */
    constructor(subscribeChanged) {
        this.targetEvent = new EventTarget();
        if (typeof (subscribeChanged) === "function")
            this.subscribeChangedCallback = subscribeChanged;
        else
            this.subscribeChangedCallback = (type, count) => {
                console.debug(`事件${type}的订阅数量为：${count}`);
            };
    }

    /**
     * 关注事件
     *
     * @param {*} event 事件名称
     * @param {*} callback 回调函数，该函数支持一个参数，即来自服务器下发的消息
     * @memberof WebEvent
     */
    on(event, callback) {
        console.debug(`关注事件：${event}`);
        this.targetEvent.addEvent(event, callback);
        this.subscribeChangedCallback(event, this.targetEvent.getHandlerCounts(event));
    }

    /**
     * 取消关注事件
     *
     * @param {*} event 事件名称
     * @param {*} callback 回调函数，该函数支持一个参数，即来自服务器下发的消息
     * @memberof WebEvent
     */
    off(event, callback) {
        console.debug(`取关事件：${event}`);
        this.targetEvent.removeEvent(event, callback);
        this.subscribeChangedCallback(event, this.targetEvent.getHandlerCounts(event));
    }

    /**
     * 触发事件
     *
     * @param {*} event
     * @param {*} data
     * @memberof WebEvent
     */
    fire(event, data) {
        this.targetEvent.fireEvent({ type: event, data: data });
    }

    /**
     * 返回当前是否和服务器连接成功
     * 
     * @returns
     * @memberof WebEvent
     */
    isConnected() {
        return Socket.readyState === 1;
    }
    /**
     * 获取当前已经被订阅的所有事件信息
     *
     * @memberof WebEvent
     */
    allsubscribeEvents() {
        return this.targetEvent.allsubscribeEvents();
    }
}

//#endregion

//#region ws相关

function openWs() {
    console.debug(`WS服务器连接成功`);
    window.webEvent.fire(SocketEvent.Connected, null);
    ping();
    if (typeof (connectCallBack) === "function") {
        var clientInfo = connectCallBack();
        if (clientInfo) {
            console.debug("正在登录到WS服务器...");
            Socket.send(`login:${clientInfo}`);
        }
    }
}

function ping() {
    Socket.send("ping");
    PingInterval = setInterval(() => {
        Socket.send("ping");
    }, 1000 * 60);//没60秒发送一次心跳
}


function onmessageWS(e) {
    if (!e.data) { return; }
    if (e.data.startsWith("welcome://")) {
        console.log(e.data.substr(10));//登录成功
        isLogined = true;
        //window.webEvent.fire(info[0], data);
        let allEvent = window.webEvent.allsubscribeEvents();
        for (let i = 0; i < allEvent.length; i++) {
            //在服务器上订阅或者取消订阅事件
            let type = allEvent[i].type;
            if (type === SocketEvent.Connected) continue;
            if (type === SocketEvent.DisConnected) continue;
            console.debug(`正在服务器上订阅事件 ${type}...`)
            let count = allEvent[i].count;
            var action = count > 0 ? "subscribe" : "unsubscribe";
            Socket.send(`${action}:${type}`);
        }
        return;
    }

    var info = e.data.split('://');
    var event = info[0];
    if (info.length >= 2) {
        info.splice(0, 1);//移除事件名称
        var data = info.join("://");//合并后续数据
        try {
            data = JSON.parse(data);
        }
        catch{ }
        window.webEvent.fire(event, data);
    }
    else {
        console.warn(`收到无法处理的WS推送数据:${e.data}`);
        window.webEvent.fire("onError", e.data);
    }
}

function onerrorWS() {
    console.warn("WS连接出错");
    clearInterval(PingInterval);
    Socket.close();
    window.webEvent.fire(SocketEvent.DisConnected, null);
    isLogined = false;
    createWS(); //重连
}

function oncloseWS() {
    clearInterval(PingInterval);
    window.webEvent.fire(SocketEvent.DisConnected, null);
    setTimeout(onerrorWS, 5000);
    console.log('WS连接已断开');
}

function createWS() {
    console.debug(`正在连接到WS服务器${defaultSettings.pushUrl}...`);
    if (Socket) {
        Socket.onopen = null;
        Socket.onmessage = null;
        Socket.onerror = null;
        Socket.onclose = null;
        Socket.close();
        Socket = null;
    }
    Socket = new WebSocket(defaultSettings.pushUrl);
    Socket.onopen = openWs;
    Socket.onmessage = onmessageWS;
    Socket.onerror = onerrorWS;
    Socket.onclose = oncloseWS;
}

function subscribeChangedHandller(type, count) {
    if (!isLogined) {
        console.warn(`尚未登录成功，无法订阅事件:${type}`);
        return;
    }
    if (count !== 0 && count !== 1) return;
    if (Socket.readyState !== 1) return;
    //在服务器上订阅或者取消订阅事件
    var action = count > 0 ? "subscribe" : "unsubscribe";
    Socket.send(`${action}:${type}`);
}

/**
 * 初始化ws连接，当连接成功之后，会回调loginCallback函数，该函数需要返回可用于登录的信息，该信息会以字符串的方式发送到服务器上，推荐使用服务器返回的token数据
 *
 * @export
 * @param {*} loginCallback
 * @returns
 */
export function initWs(loginCallback) {
    window.webEvent = new WebEvent(subscribeChangedHandller);
    if (!defaultSettings.pushUrl) {
        console.warn("当前尚未配置任何websocket服务器地址");
        return;
    }
    if (!!window.WebSocket && window.WebSocket.prototype.send) {
        connectCallBack = loginCallback;
        createWS();
    }
    else {
        alert("当前浏览器不支持我们的WEB推送服务，请升级您的浏览器或者使用Chrome浏览器！");
    }
}

/**
 * 反初始化Ws连接
 *
 * @export
 */
export function invInitWs() {
    if (Socket) {
        Socket.close();
        window.webEvent.fire(SocketEvent.DisConnected, null);
    }
    if (PingInterval) clearInterval(PingInterval);
    isLogined = false;
}

//#endregion