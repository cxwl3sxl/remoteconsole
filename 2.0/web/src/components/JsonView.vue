<template>
  <div>
    <div>
      <image-button :isChecked="view==='json'" tag="json" @click="click" title="Json视图">
        <img src="./../assets/json.png" />
      </image-button>
      <image-button :isChecked="view==='text'" tag="text" @click="click" title="文本视图">
        <img src="./../assets/text.png" />
      </image-button>
      <copy-button :content="data"></copy-button>
    </div>
    <div>
      <json-view v-if="view==='json'" :closed="true" :deep="1" :line-height="14" :data="json" />
      <div v-if="view==='text'" v-html="data"></div>
    </div>
  </div>
</template>

<script>
import ImageButton from "./ImageButton";
import jsonView from "vue-json-views";
import CopyButton from "./CopyButton";
export default {
  name: "JsonView",
  components: {
    jsonView,
    ImageButton,
    CopyButton
  },
  data: function() {
    return {
      view: "json"
    };
  },
  props: {
    data: { required: true }
  },
  computed: {
    json() {
      return JSON.parse(this.data);
    }
  },
  methods: {
    click(isChecked, view) {
      this.view = view;
    }
  }
};
</script>