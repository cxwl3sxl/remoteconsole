<template>
  <image-button
    :isChecked="false"
    class="btnCopy"
    :data-clipboard-text="content"
    title="复制文本"
    @click="copy"
  >
    <img src="./../assets/copy.png" />
  </image-button>
</template>

<script>
import Clipboard from "clipboard";
import ImageButton from "./ImageButton";
export default {
  name: "CopyButton",
  components: {
    ImageButton
  },
  props: {
    content: { type: String, required: true }
  },
  methods: {
    copy: function() {
      var clipboard = new Clipboard(".btnCopy");
      clipboard.on("success", () => {
        alert("复制成功");
        clipboard.destroy();
      });
      clipboard.on("error", () => {
        alert("该浏览器不支持自动复制，请手工复制");
        clipboard.destroy();
      });
    }
  }
};
</script>