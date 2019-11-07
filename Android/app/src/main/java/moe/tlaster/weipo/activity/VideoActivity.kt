package moe.tlaster.weipo.activity

import android.net.Uri
import android.os.Build
import android.os.Build.VERSION.SDK_INT
import android.os.Bundle
import com.google.android.exoplayer2.ExoPlayerFactory
import com.google.android.exoplayer2.SimpleExoPlayer
import com.google.android.exoplayer2.source.ProgressiveMediaSource
import com.google.android.exoplayer2.upstream.DefaultDataSourceFactory
import com.google.android.exoplayer2.util.Util
import kotlinx.android.synthetic.main.activity_video.*
import moe.tlaster.weipo.R

class VideoActivity : BaseActivity() {
    private var autoPlay: Boolean = true
    private val AUTOPLAY = "autoplay"
    private val CURRENT_WINDOW_INDEX = "current_window_index"
    private val PLAYBACK_POSITION = "playback_position"
    private var currentWindow = 0
    private var playbackPosition = 0L

    private var player: SimpleExoPlayer? = null
    override val layoutId: Int
        get() = R.layout.activity_video
    private val url by lazy {
        intent.getStringExtra("url")
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        if (savedInstanceState != null) {
            playbackPosition = savedInstanceState.getLong(PLAYBACK_POSITION, 0)
            currentWindow = savedInstanceState.getInt(CURRENT_WINDOW_INDEX, 0)
            autoPlay = savedInstanceState.getBoolean(AUTOPLAY, false)
        }
    }


    private fun initializePlayer() {
        player = ExoPlayerFactory.newSimpleInstance(this).apply{
            playWhenReady = autoPlay
            player_view.player = this
            val dataSourceFactory = DefaultDataSourceFactory(
                this@VideoActivity,
                Util.getUserAgent(this@VideoActivity, packageName)
            )
            prepare(ProgressiveMediaSource.Factory(dataSourceFactory).createMediaSource(Uri.parse(url)))
            seekTo(currentWindow, playbackPosition)
        }
    }

    private fun releasePlayer() {
        player?.let {
            // save the player state before releasing its resources
            playbackPosition = it.currentPosition
            currentWindow = it.currentWindowIndex
            autoPlay = it.playWhenReady
            it.release()

        }
        player = null
    }

    // NOTE: we initialize the player either in onStart or onResume according to API level
    // API level 24 introduced support for multiple windows to run side-by-side. So it's safe to initialize our player in onStart
    // more on Multi-Window Support here https://developer.android.com/guide/topics/ui/multi-window.html
    // Before API level 24, we wait as long as onResume (to grab system resources) before initializing player
    override fun onStart() {
        super.onStart()
        if (SDK_INT > Build.VERSION_CODES.M) {
            initializePlayer()
        }
    }

    override fun onResume() {
        super.onResume()
        if (SDK_INT <= Build.VERSION_CODES.M || player == null) {
            initializePlayer()
        }
    }

    // Before API level 24 we release player resources early
    // because there is no guarantee of onStop being called before the system terminates our app
    // remember onPause means the activity is partly obscured by something else (e.g. incoming call, or alert dialog)
    // so we do not want to be playing media while our activity is not in the foreground.
    override fun onPause() {
        super.onPause()
        if (SDK_INT <= Build.VERSION_CODES.M) {
            releasePlayer()
        }
    }

    // API level 24+ we release the player resources when the activity is no longer visible (onStop)
    // NOTE: On API 24+, onPause is still visible!!! So we do not not want to release the player resources
    // this is made possible by the new Android Multi-Window Support https://developer.android.com/guide/topics/ui/multi-window.html
    // We stop playing media on API 24+ only when our activity is no longer visible aka onStop
    override fun onStop() {
        super.onStop()
        if (SDK_INT > Build.VERSION_CODES.M) {
            releasePlayer()
        }
    }

    override fun onSaveInstanceState(outState: Bundle) {
        super.onSaveInstanceState(outState)
        if (player == null) {
            outState.putLong(PLAYBACK_POSITION, playbackPosition)
            outState.putInt(CURRENT_WINDOW_INDEX, currentWindow)
            outState.putBoolean(AUTOPLAY, autoPlay)
        }
    }
}
