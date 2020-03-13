<template>
  <div class="home">
    <img width="128" src="../assets/logo.png" />
    <div class="title">欢迎使用远程日志服务</div>
    <div class="help">
      <p>
        1. 自动
        <button @click="generateChannel()">生成</button>一个或者手动
        <button @click="inputChannel()">输入</button>一个Channel编号（可选）
      </p>
      <p>
        2.
        <button class="btnCopy" :data-clipboard-text="tobeCopyScript" @click="copyScript">复制</button>下面的脚本到你的页面即可
        <span class="code">&lt;script src="{{script}}"&gt;&lt;/script&gt;</span>
      </p>
      <p>
        3. 在本页面
        <button @click="login()">登录</button>，进入日志查看界面
      </p>
    </div>
  </div>
</template>

<script>
import Clipboard from "clipboard";
import { initWs } from "@/util/webpush";

export default {
  name: "Home",
  data() {
    return {
      script: "",
      channel: "",
      tobeCopyScript: ""
    };
  },
  mounted() {
    let preChannel = localStorage.getItem("_channelId");
    if (!preChannel) {
      preChannel = this.newId();
      localStorage.setItem("_channelId", preChannel);
    }
    this.channel = preChannel;
    this.updateScript();
  },
  methods: {
    updateScript: function() {
      this.script = `${window.location.protocol}//${window.location.host}/script/log.js?channel=${this.channel}`;
      this.tobeCopyScript = "<script src='" + this.script + "'><" + "/script>";
    },
    copyScript: function() {
      var clipboard = new Clipboard(".btnCopy");
      clipboard.on("success", () => {
        alert("复制成功");
        clipboard.destroy();
      });
      clipboard.on("error", () => {
        alert("该浏览器不支持自动复制，请手工复制");
        clipboard.destroy();
      });
    },
    newId: () => {
      return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(
        c
      ) {
        var r = (Math.random() * 16) | 0,
          v = c == "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      });
    },
    generateChannel: function() {
      let tmpChannel = this.newId();
      localStorage.setItem("_channelId", tmpChannel);
      this.channel = tmpChannel;
      this.updateScript();
    },
    login: function() {
      initWs(() => this.channel);
      this.$router.push({ path: `/LogView/${this.channel}` });
    },
    inputChannel: function() {
      let channel = window.prompt(
        "请输入您想要的Channel号，注意：相同的Channel日志是共享的。"
      );
      if (channel) {
        localStorage.setItem("_channelId", channel);
        this.channel = channel;
        this.updateScript();
      }
    }
  }
};
</script>

<style scoped>
button {
  background-color: #2196f3;
  border-color: #357ebd;
  color: #fff;
  border-radius: 5px;
  -khtml-border-radius: 10px;
  text-align: center;
  vertical-align: middle;
  border: 1px solid transparent;
  padding: 2px 10px;
}
.home {
  padding-top: 50px;
}

.title {
  margin-top: 20px;
  font-size: 20px;
  font-style: italic;
}

.help {
  margin-top: 20px;
  text-align: left;
}

.code {
  color: chocolate;
  font-style: italic;
}

.help {
  margin: 20px;
  border: solid 1px #4caf50;
  padding: 20px;
  border-radius: 10px;
}
</style>
