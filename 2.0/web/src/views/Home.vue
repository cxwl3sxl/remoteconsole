<template>
  <div class="home">
    <img width="128" src="../assets/logo.png" />
    <div class="title">欢迎使用远程日志服务</div>
    <div class="help">
      <p>
        1. 在本页面
        <a href="javascript:void(0)" @click="login">登录</a>，进入日志查看界面
      </p>
      <p>
        2. 复制下面的脚本到你的页面即可
        <span class="code">&lt;script&gt; {{script}} &lt;/script&gt;</span>
      </p>
    </div>
  </div>
</template>

<script>
import { initWs } from "@/util/webpush";

export default {
  name: "Home",
  data() {
    return {
      script: "",
      channel: ""
    };
  },
  mounted() {
    this.channel = this.newId();
    this.script = `${window.location.protocol}//${window.location.host}/script/log.js?channel=${this.channel}`;
  },
  methods: {
    newId: () => {
      return "xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx".replace(/[xy]/g, function(
        c
      ) {
        var r = (Math.random() * 16) | 0,
          v = c == "x" ? r : (r & 0x3) | 0x8;
        return v.toString(16);
      });
    },
    login: function() {
      initWs(() => this.channel);
      this.$router.push({ path: `/LogView/${this.channel}` });
      //   <div id="nav">
      //   <router-link to="/">Home</router-link>|
      //   <router-link to="/about">About</router-link>
      // </div>
    }
  }
};
</script>

<style scoped>
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
}
</style>
