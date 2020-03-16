<template>
  <div v-if="type==='array'">
    <div>ss</div>
    <div>
      <table class="my-table">
        <thead>
          <tr>
            <th v-for="col in columns" :key="col">{{col}}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in data" :key="item[columns[0]]">
            <td v-for="col in columns" :key="col">{{item[col]}}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <json-view v-else-if="type==='object'" closed="true" deep="1" line-height="14" :data="json" />
  <div v-else v-html="msg"></div>
</template>

<script>
import jsonView from "vue-json-views";
export default {
  name: "Log",
  props: {
    msg: { required: true }
  },
  components: {
    jsonView
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
    },
    json() {
      return JSON.parse(this.msg);
    },
    columns() {
      let cols = [];
      let obj = JSON.parse(this.msg);
      if (obj.length === 0) return cols;
      for (var attr in obj[0]) {
        cols.push(attr);
      }
      return cols;
    },
    data() {
      return JSON.parse(this.msg);
    }
  }
};
</script>

<style scoped>
.my-table {
  width: 100%;
  border-spacing: 0;
}

.my-table > thead > tr > th {
  border-bottom: solid 2px;
  padding: 0 5px;
}

.my-table > tbody > tr > td {
  border-bottom: solid 1px;
  padding: 0 5px;
}

.my-table > tbody > tr:nth-child(odd) > td {
  background: #eee;
}

.my-table > tbody > tr:nth-child(even) > td {
  background: #fff;
}
</style>