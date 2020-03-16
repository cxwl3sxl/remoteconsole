<template>
  <div class="dashboard">
    <div class="header">
      <div class="align-left">
        <div
          :class="'switch-btn btn-trace '+(trace?'':'disabled')"
          @click="switchLevel('trace')"
        >TRACE</div>
        <div
          :class="'switch-btn btn-debug '+(debug?'':'disabled')"
          @click="switchLevel('debug')"
        >DEBUG</div>
        <div :class="'switch-btn btn-log '+(log?'':'disabled')" @click="switchLevel('log')">LOG</div>
        <div :class="'switch-btn btn-info '+(info?'':'disabled')" @click="switchLevel('info')">INFO</div>
        <div :class="'switch-btn btn-warn '+(warn?'':'disabled')" @click="switchLevel('warn')">WARN</div>
        <div
          :class="'switch-btn btn-error '+(error?'':'disabled')"
          @click="switchLevel('error')"
        >ERROR</div>
      </div>
      <div class="align-right">
        <div class="switch-btn" @click="cleanlog()">清空</div>
        <div class="switch-btn" @click="logout()">注销</div>
      </div>
    </div>
    <div class="body">
      <div v-for="log in logdata" :key="log.id" :class="'log-item log-item-'+log.level">
        <div class="time">[[{{log.time}}]]</div>
        <div class="msg" v-html="log.msg"></div>
      </div>
    </div>
  </div>
</template>


<script>
export default {
  name: "LogView",
  data() {
    return {
      trace: true,
      debug: true,
      log: true,
      info: true,
      warn: true,
      error: true,
      logdata: []
    };
  },
  mounted() {
    if (!window.webEvent) {
      this.$router.push({ path: "/" });
      return;
    }
    //this.a = this.$route.params.channel;
    window.webEvent.on("onError", this.webEventError);
    window.webEvent.on("RemoteConsole.LogRequest", this.appendLog);
  },
  destroyed() {
    if (window.webEvent) {
      window.webEvent.off("RemoteConsole.LogRequest", this.appendLog);
      window.webEvent.off("onError", this.webEventError);
    }
  },
  methods: {
    switchLevel: function(level) {
      switch (level) {
        case "trace":
          this.trace = !this.trace;
          break;
        case "debug":
          this.debug = !this.debug;
          break;
        case "log":
          this.log = !this.log;
          break;
        case "info":
          this.info = !this.info;
          break;
        case "warn":
          this.warn = !this.warn;
          break;
        case "error":
          this.error = !this.error;
          break;
      }
    },
    cleanlog: function() {
      if (window.confirm("确定要清空所有日志么？")) {
        this.logdata = [];
      }
    },
    appendLog: function(loginfo) {
      if (loginfo.Level === "log" && !this.log) return;
      if (loginfo.Level === "info" && !this.info) return;
      if (loginfo.Level === "warn" && !this.warn) return;
      if (loginfo.Level === "error" && !this.error) return;
      if (loginfo.Level === "trace" && !this.trace) return;
      if (loginfo.Level === "debug" && !this.debug) return;
      this.logdata.unshift({
        id: this.logdata.length + 1,
        level: loginfo.Level,
        msg: loginfo.Message,
        time: loginfo.Time
      });
    },
    logout: function() {
      this.$router.push({ path: "/" });
    },
    webEventError: function(msg) {
      this.appendLog({
        Level: "error",
        Message: `数据无法解析：${msg}`,
        Time: new Date()
      });
    }
  }
};
</script>

<style scoped>
.body {
  padding-top: 46px;
}
.align-left {
  float: left;
}
.align-right {
  float: right;
}
.dashboard {
  font-size: 14px;
  text-align: left;
}
.header {
  background: #eee;
  border: solid 1px #ddd;
  box-shadow: #ddd 0px 2px 5px;
  margin-bottom: 5px;
  padding: 2px;
  height: 35px;
  position: fixed;
  right: 0;
  left: 0;
  top: 0;
}
.switch-btn {
  background: #2196f3;
  margin: 2px 1px;
  padding: 2px;
  cursor: pointer;
  display: inline-block;
  width: 60px;
  text-align: center;
  line-height: 25px;
  border: solid 1px #0d47a1;
  color: #fff;
  cursor: pointer;
}
.btn-trace {
  border-color: #bcaaa4;
  background-color: #795548;
}
.btn-debug {
  border-color: #9575cd;
  background-color: #5e35b1;
}
.btn-log {
  border-color: #62656a;
  background-color: #909399;
}
.btn-info {
  border-color: #007af5;
  background-color: #409eff;
}
.btn-warn {
  border-color: #b57617;
  background-color: #e6a23c;
}
.btn-error {
  border-color: #e51010;
  background-color: #f56c6c;
}
.disabled {
  opacity: 0.5;
}
.log-item {
  font-size: 12px;
  padding: 0 5px;
}
.time {
  padding-right: 10px;
  float: left;
}
.msg {
  display: inline-block;
  width: calc(100vw - 150px);
}

.log-item-trace {
  color: #795548;
}
.log-item-debug {
  color: #5e35b1;
}
.log-item-log {
  color: #62656a;
}
.log-item-info {
  color: #007af5;
}
.log-item-warn {
  color: #b57617;
}
.log-item-error {
  color: #e51010;
}
</style>