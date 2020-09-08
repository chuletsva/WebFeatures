import Vue from "vue";

class EventBus {
    bus = new Vue();

    function subscribe(event, handler) {
        bus.$on(event, handler);
    }

    function publish(event, args) {
        bus.$emit(event, args);
    }
}

export default new EventBus();