<template>
  <table-view v-if="type==='array'" :data="msg"></table-view>
  <json-view v-else-if="type==='object'" :data="msg" />
  <div v-else v-html="msg"></div>
</template>

<script>
import JsonView from "./JsonView";
import TableView from "./TableView";
export default {
  name: "Log",
  props: {
    msg: { required: true }
  },
  components: {
    JsonView,
    TableView
  },
  computed: {
    type() {
      try {
        let obj = JSON.parse(this.msg);
        if (obj instanceof Array) return "array";
        return typeof obj;
      } catch {
        return typeof this.msg;
      }
    }
  }
};
</script>