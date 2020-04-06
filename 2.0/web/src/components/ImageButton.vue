<template>
  <div :class="'normal-btn '+(isChecked?'check-btn':'')" @click="click">
    <slot />
  </div>
</template>

<script>
export default {
  name: "ImageButton",
  props: {
    isChecked: { type: Boolean, required: true },
    tag: { required: false }
  },
  methods: {
    click() {
      this.$emit("click", this.isChecked, this.tag);
    }
  }
};
</script>

<style scoped>
.normal-btn {
  display: inline-block;
  padding: 1px 2px;
  border: solid 1px transparent;
  margin-right: 2px;
}

.check-btn {
  border: solid 1px;
}
</style>