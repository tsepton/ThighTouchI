<script>
	import Main from '$lib/components/Main.svelte';
</script>

<div id="background">
	<Main></Main>
</div>

<style>
	:global(body) {
		font-family: 'Roboto', sans-serif;
		margin: 0;
		padding: 0;
		height: 100vh;
	}

	#background {
		background: linear-gradient(120deg, #D3D4D9, #FFF9FB, #424C55);
		background-size: 350% 350%;

		-webkit-animation: AnimationName 50s ease infinite;
		-moz-animation: AnimationName 50s ease infinite;
		animation: AnimationName 50s ease infinite;
	}

	@-webkit-keyframes AnimationName {
		0% {
			background-position: 0% 30%;
		}
		50% {
			background-position: 100% 71%;
		}
		100% {
			background-position: 0% 30%;
		}
	}
	@-moz-keyframes AnimationName {
		0% {
			background-position: 0% 30%;
		}
		50% {
			background-position: 100% 71%;
		}
		100% {
			background-position: 0% 30%;
		}
	}
	@keyframes AnimationName {
		0% {
			background-position: 0% 30%;
		}
		50% {
			background-position: 100% 71%;
		}
		100% {
			background-position: 0% 30%;
		}
	}

	:global(.card) {
		background: rgba(255, 255, 255, 0.5);
		border-radius: 16px;
		box-shadow: 0 4px 30px rgba(0, 0, 0, 0.1);
		backdrop-filter: blur(5px);
		-webkit-backdrop-filter: blur(5px);
		border: 1px solid rgba(255, 255, 255, 0.3);
		padding-top: 2rem;
		padding-bottom: 2rem;
	}
</style>
