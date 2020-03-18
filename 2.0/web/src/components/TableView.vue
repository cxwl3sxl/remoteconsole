<template>
  <div>
    <div>
      <image-button :isChecked="view==='table'" tag="table" @click="click" title="表格视图">
        <img src="./../assets/table.png" />
      </image-button>
      <image-button :isChecked="view==='json'" tag="json" @click="click" title="Json视图">
        <img src="./../assets/json.png" />
      </image-button>
      <image-button :isChecked="view==='text'" tag="text" @click="click" title="文本视图">
        <img src="./../assets/text.png" />
      </image-button>
      <copy-button :content="data"></copy-button>
    </div>
    <div>
      <table v-if="view==='table'" class="my-table">
        <thead>
          <tr>
            <th v-for="col in columns" :key="col">{{col}}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="item in rows" :key="item[columns[0]]">
            <td v-for="col in columns" :key="col">{{item[col]}}</td>
          </tr>
        </tbody>
      </table>
      <json-view v-if="view==='json'" :closed="true" :deep="1" :line-height="14" :data="rows" />
      <div v-if="view==='text'" v-html="data"></div>
    </div>
  </div>
</template>

<script>
import ImageButton from "./ImageButton";
import CopyButton from "./CopyButton";
import jsonView from "vue-json-views";
export default {
  name: "TableView",
  props: {
    data: { required: true }
  },
  data: function() {
    return {
      view: "table"
    };
  },
  components: {
    ImageButton,
    jsonView,
    CopyButton
  },
  computed: {
    columns() {
      let cols = [];
      let obj = JSON.parse(this.data);
      if (obj.length === 0) return cols;
      for (var attr in obj[0]) {
        cols.push(attr);
      }
      return cols;
    },
    rows() {
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